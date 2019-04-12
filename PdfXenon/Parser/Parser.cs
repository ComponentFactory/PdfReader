using System;
using System.Collections.Generic;
using System.IO;

namespace PdfXenon.Standard
{
    public class Parser : IDisposable
    {
        private bool _disposed;

        public event EventHandler<ParseResolveEventArgs> ResolveReference;

        public Parser(Stream stream)
        {
            Tokenizer = new Tokenizer(stream);
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
            if (!c.Comment.StartsWith("%PDF"))
                throw new ApplicationException("PDF Header must start with '%PDF'.");

            string[] splits = c.Comment.Substring(5).Split('.');
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
            ThrowIfNot<TokenKeyword>(t);

            TokenKeyword keyword = (TokenKeyword)t;
            if (keyword.Keyword != ParseKeyword.XRef)
                throw new ApplicationException($"Cross-reference table has keyword {keyword.Keyword.ToString()} instead of 'xref' at position {t.Position}.");

            List<TokenXRefEntry> entries = new List<TokenXRefEntry>();
            ParseXRefSections(entries);
            return entries;
        }

        public void ParseXRefSections(List<TokenXRefEntry> entries)
        {
            while (true)
            {
                TokenObject t = Tokenizer.GetToken();
                ThrowOnError(t);

                // Cross-reference table ends when we find a 'trailer' keyword instead of another section
                if ((t is TokenKeyword) && (((TokenKeyword)t).Keyword == ParseKeyword.Trailer))
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
                for (int i = 0, id = start.Integer; i < length.Integer; i++, id++)
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
            if (!(t is TokenKeyword) || (((TokenKeyword)t).Keyword != ParseKeyword.Trailer))
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
            if (!(v is TokenKeyword) || ((v as TokenKeyword).Keyword != ParseKeyword.Obj))
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

            if (keyword.Keyword == ParseKeyword.EndObj)
                return new ParseIndirectObject(t as TokenInteger, u as TokenInteger, obj);
            else if (keyword.Keyword == ParseKeyword.Stream)
            {
                ParseDictionary dictionary = obj as ParseDictionary;
                if (dictionary == null)
                    throw new ApplicationException($"Stream must be preceded by a dictionary at position {v.Position}.");

                if (!dictionary.ContainsName("Length"))
                    throw new ApplicationException($"Stream dictionary must contain a 'Length' entry at position {v.Position}.");

                ParseDictEntry entry = dictionary["Length"];
                ParseObject lengthObj = entry.Object;

                // Resolve any object reference
                ParseObjectReference reference = lengthObj as ParseObjectReference;
                if (reference != null)
                    lengthObj = OnResolveReference(reference);

                ParseInteger length = lengthObj as ParseInteger;
                if (length == null)
                    throw new ApplicationException($"Stream dictionary has a 'Length' entry that is not an integer entry at position {v.Position}.");

                if (length.Integer < 0)
                    throw new ApplicationException($"Stream dictionary has a 'Length' less than 0 at position {v.Position}.");

                byte[] bytes = Tokenizer.GetBytes(length.Integer);
                if (bytes == null)
                    throw new ApplicationException($"Cannot read in expected {length.Integer} bytes from stream at position {v.Position}.");

                // Stream contents must be followed by 'endstream'
                v = Tokenizer.GetToken();
                ThrowOnEmptyOrError(v);

                keyword = (TokenKeyword)v;
                if (keyword == null)
                    throw new ApplicationException($"Stream has missing 'endstream' after content at at position {v.Position}.");

                if (keyword.Keyword != ParseKeyword.EndStream)
                    throw new ApplicationException($"Stream has unexpected keyword {keyword.Keyword} instead of 'endstream' at position {v.Position}.");

                // Stream contents must be followed by 'endobj'
                v = Tokenizer.GetToken();
                ThrowOnEmptyOrError(v);

                keyword = (TokenKeyword)v;
                if (keyword == null)
                    throw new ApplicationException($"Indirect object has missing 'endobj' at position { v.Position }.");

                if (keyword.Keyword != ParseKeyword.EndObj)
                    throw new ApplicationException($"Indirect object has unexpected keyword {keyword.Keyword} instead of 'endobj' at position {v.Position}.");

                return new ParseIndirectObject(t as TokenInteger, u as TokenInteger, new ParseStream(dictionary, bytes));
            }
            else
                throw new ApplicationException($"Indirect object has unexpected keyword {keyword.Keyword} at position {v.Position}.");
        }

        public ParseObject ParseObject()
        {
            Tokenizer.IgnoreComments = true;
            TokenObject t = Tokenizer.GetToken();
            ThrowOnEmptyOrError(t);

            if (t is TokenName)
                return new ParseName(t as TokenName);
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
                    if ((t3 is TokenKeyword) && (((TokenKeyword)t3).Keyword == ParseKeyword.R))
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
                return new ParseArray(t.Position, objects);
            }
            else if (t is TokenDictionaryOpen)
            {
                Dictionary<string, ParseDictEntry> entries = new Dictionary<string, ParseDictEntry>();

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
                        throw new ApplicationException($"Dictionary key must be a name instead of {name.GetType().Name} at position {name.Position}.");

                    value2 = ParseObject();
                    if (value2 == null)
                        throw new ApplicationException($"Dictionary value missing for key {name.Name} at position {name.Position}.");
                    else
                        ThrowOnEmptyOrError(t);

                    // If key already exists then simply overwrite it with latest value
                    entries[name.Name] = new ParseDictEntry() { Name = name, Object = value2 };
                }

                ThrowIfNot<TokenDictionaryClose>(Tokenizer.GetToken());
                return new ParseDictionary(t.Position, entries);
            }
            else if (t is TokenKeyword)
            {
                switch ((t as TokenKeyword).Keyword)
                {
                    case ParseKeyword.True:
                    case ParseKeyword.False:
                        return new ParseBoolean(t as TokenKeyword);
                    case ParseKeyword.Null:
                        return new ParseNull(t as TokenKeyword);
                }
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