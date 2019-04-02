using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Parser
    {
        public Parser(Stream stream)
        {
            Tokenizer = new Tokenizer(stream);
        }

        public PdfObject ParseObject()
        {
            TokenBase t = Tokenizer.GetToken();
            ThrowOnEmptyOrError(t);

            if (t is TokenName)
            {
                return new PdfName(t as TokenName);
            }
            else if (t is TokenNumeric)
            {
                return new PdfNumeric(t as TokenNumeric);
            }
            else if (t is TokenHexString)
            {
                return new PdfString(t as TokenHexString);
            }
            else if (t is TokenLiteralString)
            {
                return new PdfString(t as TokenLiteralString);
            }
            else if (t is TokenArrayOpen)
            {
                List<PdfObject> objects = new List<PdfObject>();

                PdfObject entry = null;
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
                return new PdfArray(t.Position, objects);
            }
            else if (t is TokenDictionaryOpen)
            {
                PdfObject value1 = null;
                PdfObject value2 = null;
                while (true)
                {
                    value1 = ParseObject();
                    if (value1 == null)
                        break;
                    else
                        ThrowOnEmptyOrError(t);

                    PdfName name = value1 as PdfName;
                    if (name == null)
                        throw new ApplicationException($"Dictionary key must be a name instead of {name.GetType().Name} at position {name.Position}.");

                    value2 = ParseObject();
                    if (value2 == null)
                        throw new ApplicationException($"Dictionary value missing for key {name.Name} at position {name.Position}.");
                    else
                        ThrowOnEmptyOrError(t);
                }

                ThrowIfNot<TokenDictionaryClose>(Tokenizer.GetToken());
                return new PdfDictionary(t.Position);
            }
            else if (t is TokenKeyword)
            {
                switch ((t as TokenKeyword).Keyword)
                {
                    case PdfKeyword.True:
                    case PdfKeyword.False:
                        return new PdfBoolean(t as TokenKeyword);
                    case PdfKeyword.Null:
                        return new PdfNull(t as TokenKeyword);
                }
            }

            // Token is not one that starts an object, so put the token back
            Tokenizer.CachedToken = t;
            return null;

            //if (t is TokenDictionaryOpen)
            //{
            //    while (true)
            //    {
            //        t = Tokenizer.GetToken();
            //        if (t is TokenName)
            //        {
            //            ParseObject();
            //        }
            //        else if (t is TokenDictionaryClose)
            //        {
            //            return;
            //        }
            //        else
            //            ThrowOnUnexpected(t);
            //    }
            //}
            //else if (t is TokenName)
            //{
            //}
            //else if (t is TokenNumeric)
            //{
            //    TokenNumeric n = (TokenNumeric)t;
            //    if (n.IsInteger)
            //    {
            //        If followed by another number then it is an object reference
            //        t = Tokenizer.GetToken();
            //        if (t is TokenNumeric)
            //        {
            //            int identifier = n.Integer.Value;

            //            n = (TokenNumeric)t;
            //            if (n.IsInteger)
            //            {
            //                int generation = n.Integer.Value;

            //                TokenKeyword k = ThrowIfNot<TokenKeyword>(Tokenizer.GetToken());
            //                if (k.Keyword != PdfKeyword.R)
            //                    throw new ApplicationException($"Object reference must use keyword 'R', at position {k.Position}.");
            //            }
            //            else
            //                throw new ApplicationException($"Object reference must have a generation that is an integer, at position {n.Position}.");
            //        }
            //        else
            //            Tokenizer.CachedToken = t;
            //    }
            //}
            //else
            //    ThrowOnUnexpected(t);
        }

        //public void TestParse()
        //{
        //    try
        //    {
        //        ParseHeader(out int major, out int minor);
        //        ParseIndirectObjects();
        //    }
        //    catch(ApplicationException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        //private void ParseHeader(out int major, out int minor)
        //{
        //    // The header is a comment token
        //    Tokenizer.IgnoreComments = false;
        //    TokenBase t = Tokenizer.GetToken();
        //    if (!(t is TokenComment))
        //        throw new ApplicationException("Missing PDF header.");

        //    TokenComment c = (TokenComment)t;
        //    if (!c.Comment.StartsWith("%PDF"))
        //        throw new ApplicationException("PDF Header must start with %PDF text.");

        //    string[] splits = c.Comment.Substring(5).Split('.');
        //    if (splits.Length != 2)
        //        throw new ApplicationException("PDF Header must have a major.minor version number.");

        //    if (!int.TryParse(splits[0].Trim(), out major))
        //        throw new ApplicationException("Could not parse the header major version number.");

        //    if (!int.TryParse(splits[1].Trim(), out minor))
        //        throw new ApplicationException("Could not parse the header minor version number.");
        //}

        //private void ParseIndirectObjects()
        //{
        //    // Ignore any comments before the next real token
        //    Tokenizer.IgnoreComments = true;
        //    TokenBase t = Tokenizer.GetToken();
        //    ThrowOnEmptyOrError(t);

        //    // Indirect objects start with an integer number
        //    if (t is TokenNumeric)
        //    {
        //        TokenNumeric n = (TokenNumeric)t;
        //        if (!n.IsInteger)
        //            throw new ApplicationException($"Indirect object must have integer identifier, at position {n.Position}.");

        //        int identifier = n.Integer.Value;

        //        n = ThrowIfNot<TokenNumeric>(Tokenizer.GetToken());
        //        if (!n.IsInteger)
        //            throw new ApplicationException($"Indirect object must have integer generation, at position {n.Position}.");

        //        int generation = n.Integer.Value;

        //        TokenKeyword k = ThrowIfNot<TokenKeyword>(Tokenizer.GetToken());
        //        if (k.Keyword == PdfKeyword.Obj)
        //        {
        //            ParseObject();

        //            k = ThrowIfNot<TokenKeyword>(Tokenizer.GetToken());
        //            if (k.Keyword != PdfKeyword.EndObj)
        //                throw new ApplicationException($"Indirect object has missing endobj, at position {n.Position}.");
        //        }
        //        else if (k.Keyword == PdfKeyword.R)
        //        {
        //            // todo
        //        }
        //        else
        //            throw new ApplicationException($"Indirect object must specify an object or reference as content, at position {n.Position}.");
        //    }
        //    else if (t is TokenKeyword)
        //    {
        //        // the xref keyword means we are finished
        //    }
        //    else
        //        ThrowOnUnexpected(t);
        //}

        private T ThrowIfNot<T>(TokenBase t) where T : TokenBase
        {
            if (t is TokenError)
                throw new ApplicationException(t.ToString());
            else if (t is TokenEmpty)
                throw new ApplicationException("Unexpected end of PDF document.");
            else if (!(t is T))
                throw new ApplicationException($"Found {t.GetType().Name} instead of {typeof(T).Name} at position {t.Position}.");

            return (T)t;
        }

        private void ThrowOnEmptyOrError(TokenBase t)
        {
            if (t is TokenError)
                throw new ApplicationException(t.ToString());
            else if (t is TokenEmpty)
                throw new ApplicationException("Unexpected end of PDF document.");
        }

        private void ThrowOnUnexpected(TokenBase t)
        {
            throw new ApplicationException($"Unexpected PDF content at position {t.Position}.");
        }

        private Tokenizer Tokenizer { get; set; }
    }
}
