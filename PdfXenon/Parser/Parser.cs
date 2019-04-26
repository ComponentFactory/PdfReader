using System;
using System.Collections.Generic;
using System.IO;

namespace PdfXenon.Standard
{
    public class Parser : IDisposable
    {
        private static Dictionary<string, ParseName> _uniqueNames = new Dictionary<string, ParseName>();
        private static Dictionary<string, ParseIdentifier> _uniqueIdentifiers = new Dictionary<string, ParseIdentifier>();

        private bool _disposed;

        public event EventHandler<ParseResolveEventArgs> ResolveReference;

        public Parser(Stream stream, bool allowIdentifiers = false)
        {
            Tokenizer = new Tokenizer(stream) { AllowIdentifiers = allowIdentifiers };
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void ParseHeader(out int major, out int minor)
        {
            // The header is a comment token
            Tokenizer.IgnoreComments = false;
            TokenObject t = Tokenizer.GetToken();
            if (!(t is TokenComment))
                throw new ApplicationException("Missing PDF header.");

            TokenComment c = (TokenComment)t;
            if (!c.Value.StartsWith("%PDF"))
                throw new ApplicationException("PDF Header must start with '%PDF'.");

            string[] splits = c.Value.Substring(5).Split('.');
            if (splits.Length != 2)
                throw new ApplicationException("PDF Header must have a <major>.<minor> version number.");

            if (!int.TryParse(splits[0].Trim(), out major))
                throw new ApplicationException("Could not parse the header major version number.");

            if (!int.TryParse(splits[1].Trim(), out minor))
                throw new ApplicationException("Could not parse the header minor version number.");

            Tokenizer.IgnoreComments = true;
        }

        public long ParseXRefOffset()
        {
            return Tokenizer.GetXRefOffset();
        }

        public List<TokenXRefEntry> ParseXRef(long position)
        {
            Tokenizer.Position = position;
            return ParseXRef();
        }

        public List<TokenXRefEntry> ParseXRef()
        {
            Tokenizer.IgnoreComments = true;
            TokenObject t = Tokenizer.GetToken();
            TokenKeyword keyword = t as TokenKeyword;

            if ((keyword == null) || (keyword.Value != ParseKeyword.XRef))
            {
                // Scan entire source creating XRef entries for each indirect object
                return IndirectObjectsToXRef();
            }
            else
            {
                List<TokenXRefEntry> entries = new List<TokenXRefEntry>();
                ParseXRefSections(entries);
                return entries;
            }
        }

        public List<TokenXRefEntry> IndirectObjectsToXRef()
        {
            List<TokenXRefEntry> entries = new List<TokenXRefEntry>();

            // Start scanning from beginning of the source
            Tokenizer.IgnoreComments = false;
            Tokenizer.Position = 0;

            long lastTrailer = -1;

            do
            {
                TokenObject t1 = Tokenizer.GetToken();
                if (t1 is TokenInteger)
                {
                    TokenInteger t2 = Tokenizer.GetToken() as TokenInteger;
                    if (t2 != null)
                    {
                        TokenKeyword t3 = Tokenizer.GetToken() as TokenKeyword;
                        if ((t3 != null) && (t3.Value == ParseKeyword.Obj))
                        {
                            TokenInteger id = (TokenInteger)t1;
                            entries.Add(new TokenXRefEntry(t1.Position, id.Value, t2.Value, t1.Position, true));
                        }
                    }
                }
                else if ((t1 is TokenKeyword) && ((TokenKeyword)t1).Value == ParseKeyword.Trailer)
                    lastTrailer = t1.Position;

            } while (Tokenizer.GotoNextLine());

            // Leave with the position on the last 'trailer' as caller will then parse it
            if (lastTrailer >= 0)
                Tokenizer.Position = lastTrailer;

            return entries;
        }

        public void ParseXRefSections(List<TokenXRefEntry> entries)
        {
            while (true)
            {
                TokenObject t = Tokenizer.GetToken();
                ThrowOnError(t);

                // Cross-reference table ends when we find a 'trailer' keyword instead of another section
                if ((t is TokenKeyword) && (((TokenKeyword)t).Value == ParseKeyword.Trailer))
                {
                    Tokenizer.PushToken(t);
                    return;
                }

                // Section starts with an integer object number
                TokenInteger start = t as TokenInteger;
                if (start == null)
                    throw new ApplicationException($"Cross-reference section number must be an integer at position {t.Position}.");

                t = Tokenizer.GetToken();
                ThrowOnError(t);

                // Section then has an integer length number
                TokenInteger length = t as TokenInteger;
                if (length == null)
                    throw new ApplicationException($"Cross-reference section length must be an integer at position {t.Position}.");

                // Load each line in the section
                for (int i = 0, id = start.Value; i < length.Value; i++, id++)
                {
                    TokenObject entry = Tokenizer.GetXRefEntry(id);
                    ThrowOnError(entry);
                    entries.Add((TokenXRefEntry)entry);
                }
            }
        }

        public ParseDictionary ParseTrailer()
        {
            Tokenizer.IgnoreComments = true;
            TokenObject t = Tokenizer.GetToken();
            ThrowOnError(t);

            // Cross-reference table ends when we find a 'trailer' keyword instead of another section
            if (!(t is TokenKeyword) || (((TokenKeyword)t).Value != ParseKeyword.Trailer))
                throw new ApplicationException($"Trailer section must start with the 'trailer' keyword at position {t.Position}.");

            ParseObject obj = ParseObject();
            if ((obj == null) || !(obj is ParseDictionary))
                throw new ApplicationException($"Trailer section must contain a dictionary at position {t.Position}.");

            return (ParseDictionary)obj;
        }

        public ParseIndirectObject ParseIndirectObject(long position)
        {
            long restore = Tokenizer.Position;

            // Set correct position for parsing the randomly positioned object
            Tokenizer.Position = position;
            ParseIndirectObject ret = ParseIndirectObject();

            // Must restore original position so caller can continue from where they left off
            Tokenizer.Position = restore;

            return ret;
        }

        public ParseIndirectObject ParseIndirectObject()
        {
            Tokenizer.IgnoreComments = true;
            TokenObject t = Tokenizer.GetToken();
            ThrowOnEmptyOrError(t);

            // Indirect object starts with an integer, the object identifier
            if (!(t is TokenInteger))
            {
                Tokenizer.PushToken(t);
                return null;
            }

            // Second is another integer, the generation number
            TokenObject u = Tokenizer.GetToken();
            ThrowOnEmptyOrError(u);

            if (!(u is TokenInteger))
            {
                Tokenizer.PushToken(t);
                Tokenizer.PushToken(u);
                return null;
            }

            // This is the keyword 'obj'
            TokenObject v = Tokenizer.GetToken();
            ThrowOnEmptyOrError(v);
            if (!(v is TokenKeyword) || ((v as TokenKeyword).Value != ParseKeyword.Obj))
            {
                Tokenizer.PushToken(t);
                Tokenizer.PushToken(u);
                Tokenizer.PushToken(v);
                return null;
            }

            // Get actual object that is the content
            ParseObject obj = ParseObject();
            if (obj == null)
                throw new ApplicationException($"Indirect object has missing content at position {t.Position}.");

            // Must be followed by either 'endobj' or 'stream'
            v = Tokenizer.GetToken();
            ThrowOnEmptyOrError(v);

            TokenKeyword keyword = (TokenKeyword)v;
            if (keyword == null)
                throw new ApplicationException($"Indirect object has missing 'endobj or 'stream' at position {v.Position}.");

            if (keyword.Value == ParseKeyword.EndObj)
                return new ParseIndirectObject(t as TokenInteger, u as TokenInteger, obj);
            else if (keyword.Value == ParseKeyword.Stream)
            {
                ParseDictionary dictionary = obj as ParseDictionary;
                if (dictionary == null)
                    throw new ApplicationException($"Stream must be preceded by a dictionary at position {v.Position}.");

                if (!dictionary.ContainsName("Length"))
                    throw new ApplicationException($"Stream dictionary must contain a 'Length' entry at position {v.Position}.");

                ParseObject lengthObj = dictionary["Length"];

                // Resolve any object reference
                ParseObjectReference reference = lengthObj as ParseObjectReference;
                if (reference != null)
                    lengthObj = OnResolveReference(reference);

                ParseInteger length = lengthObj as ParseInteger;
                if (length == null)
                    throw new ApplicationException($"Stream dictionary has a 'Length' entry that is not an integer entry at position {v.Position}.");

                if (length.Value < 0)
                    throw new ApplicationException($"Stream dictionary has a 'Length' less than 0 at position {v.Position}.");

                byte[] bytes = Tokenizer.GetBytes(length.Value);
                if (bytes == null)
                    throw new ApplicationException($"Cannot read in expected {length.Value} bytes from stream at position {v.Position}.");

                // Stream contents must be followed by 'endstream'
                v = Tokenizer.GetToken();
                ThrowOnEmptyOrError(v);

                keyword = (TokenKeyword)v;
                if (keyword == null)
                    throw new ApplicationException($"Stream has missing 'endstream' after content at at position {v.Position}.");

                if (keyword.Value != ParseKeyword.EndStream)
                    throw new ApplicationException($"Stream has unexpected keyword {keyword.Value} instead of 'endstream' at position {v.Position}.");

                // Stream contents must be followed by 'endobj'
                v = Tokenizer.GetToken();
                ThrowOnEmptyOrError(v);

                keyword = (TokenKeyword)v;
                if (keyword == null)
                    throw new ApplicationException($"Indirect object has missing 'endobj' at position { v.Position }.");

                if (keyword.Value != ParseKeyword.EndObj)
                    throw new ApplicationException($"Indirect object has unexpected keyword {keyword.Value} instead of 'endobj' at position {v.Position}.");

                return new ParseIndirectObject(t as TokenInteger, u as TokenInteger, new ParseStream(dictionary, bytes));
            }
            else
                throw new ApplicationException($"Indirect object has unexpected keyword {keyword.Value} at position {v.Position}.");
        }

        public ParseObject ParseObject(bool allowEmpty = false)
        {
            Tokenizer.IgnoreComments = true;
            TokenObject t = Tokenizer.GetToken();

            if (allowEmpty && (t is TokenEmpty))
                return null;
            else
                ThrowOnEmptyOrError(t);

            if (t is TokenName)
            {
                // Store one instance of each unique name to minimize memory footprint
                TokenName tokenName = (TokenName)t;
                if (!_uniqueNames.TryGetValue(tokenName.Value, out ParseName parseName))
                {
                    parseName = new ParseName(tokenName.Value);
                    _uniqueNames.Add(tokenName.Value, parseName);
                }

                return parseName;
            }
            else if (t is TokenInteger)
            {
                TokenObject t2 = Tokenizer.GetToken();
                ThrowOnError(t2);

                // An object reference has a second integer, the generation number
                if (t2 is TokenInteger)
                {
                    TokenObject t3 = Tokenizer.GetToken();
                    ThrowOnError(t3);

                    // An object reference has a third value which is the 'R' keyword
                    if ((t3 is TokenKeyword) && (((TokenKeyword)t3).Value == ParseKeyword.R))
                        return new ParseObjectReference(t as TokenInteger, t2 as TokenInteger);

                    Tokenizer.PushToken(t3);
                }

                Tokenizer.PushToken(t2);

                return new ParseInteger(t as TokenInteger);
            }
            else if (t is TokenReal)
                return new ParseReal(t as TokenReal);
            else if (t is TokenStringHex)
                return new ParseString(t as TokenStringHex);
            else if (t is TokenStringLiteral)
                return new ParseString(t as TokenStringLiteral);
            else if (t is TokenArrayOpen)
            {
                List<ParseObject> objects = new List<ParseObject>();

                ParseObject entry = null;
                while (true)
                {
                    entry = ParseObject();
                    if (entry == null)
                        break;
                    else
                        ThrowOnEmptyOrError(t);

                    objects.Add(entry);
                }

                ThrowIfNot<TokenArrayClose>(Tokenizer.GetToken());
                return new ParseArray(objects);
            }
            else if (t is TokenDictionaryOpen)
            {
                List<string> names = new List<string>();
                List<ParseObject> entries = new List<ParseObject>();

                ParseObject value1 = null;
                ParseObject value2 = null;
                while (true)
                {
                    value1 = ParseObject();
                    if (value1 == null)
                        break;
                    else
                        ThrowOnEmptyOrError(t);

                    // Key value must be a Name
                    ParseName name = value1 as ParseName;
                    if (name == null)
                        throw new ApplicationException($"Dictionary key must be a name instead of {name.GetType().Name}.");

                    value2 = ParseObject();
                    if (value2 == null)
                        throw new ApplicationException($"Dictionary value missing for key {name.Value}.");
                    else
                        ThrowOnEmptyOrError(t);

                    names.Add(name.Value);
                    entries.Add(value2);
                }

                ThrowIfNot<TokenDictionaryClose>(Tokenizer.GetToken());
                return new ParseDictionary(names, entries);
            }
            else if (t is TokenKeyword)
            {
                switch ((t as TokenKeyword).Value)
                {
                    case ParseKeyword.True:
                    case ParseKeyword.False:
                        return new ParseBoolean(t as TokenKeyword);
                    case ParseKeyword.Null:
                        return new ParseNull();
                }
            }
            else if (t is TokenIdentifier)
            {
                // Store one instance of each unique identifier to minimize memory footprint
                TokenIdentifier tokenIdentifier = (TokenIdentifier)t;
                if (!_uniqueIdentifiers.TryGetValue(tokenIdentifier.Value, out ParseIdentifier parseIdentifier))
                {
                    parseIdentifier = new ParseIdentifier(tokenIdentifier.Value);
                    _uniqueIdentifiers.Add(tokenIdentifier.Value, parseIdentifier);

                }

                return parseIdentifier;
            }

            // Token is not one that starts an object, so put the token back
            Tokenizer.PushToken(t);
            return null;
        }

        protected virtual ParseObject OnResolveReference(ParseObjectReference reference)
        {
            ParseResolveEventArgs args = new ParseResolveEventArgs() { Id = reference.Id, Gen = reference.Gen };
            ResolveReference?.Invoke(this, args);
            return args.Object;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Tokenizer.Dispose();
                    Tokenizer = null;
                }

                _disposed = true;
            }
        }

        private T ThrowIfNot<T>(TokenObject t) where T : TokenObject
        {
            if (t is TokenError)
                throw new ApplicationException(t.ToString());
            else if (t is TokenEmpty)
                throw new ApplicationException("Unexpected end of PDF document.");
            else if (!(t is T))
                throw new ApplicationException($"Found {t.GetType().Name} instead of {typeof(T).Name} at position {t.Position}.");

            return (T)t;
        }

        private void ThrowOnError(TokenObject t)
        {
            if (t is TokenError)
                throw new ApplicationException(t.ToString());
        }

        private void ThrowOnEmptyOrError(TokenObject t)
        {
            if (t is TokenError)
                throw new ApplicationException(t.ToString());
            else if (t is TokenEmpty)
                throw new ApplicationException("Unexpected end of PDF document.");
        }

        private Tokenizer Tokenizer { get; set; }
    }
}