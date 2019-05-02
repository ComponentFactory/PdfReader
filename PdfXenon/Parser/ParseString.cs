using System;
using System.Globalization;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseString : ParseObject
    {
        public ParseString(TokenString token)
        {
            Token = token;
        }

        public string Value
        {
            get { return Token.Resolved; }
        }

        public byte[] ValueAsBytes
        {
            get { return Token.ResolvedAsBytes; }
        }

        public string BytesToString(byte[] bytes)
        {
            return Token.BytesToString(bytes);
        }

        private TokenString Token { get; set; }
    }
}
