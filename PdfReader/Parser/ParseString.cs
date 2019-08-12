using System;
using System.Globalization;
using System.Text;

namespace PdfReader
{
    public class ParseString : ParseObjectBase
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
