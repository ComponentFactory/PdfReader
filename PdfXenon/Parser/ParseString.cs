using System;
using System.Globalization;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseString : ParseObject
    {
        public ParseString(TokenString token)
            : base(token.Position)
        {
            Token = token;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value;
            sb.Append(output);
            return indent + output.Length;
        }

        public string Value
        {
            get { return Token.ResolvedAsString; }
        }

        public byte[] ValueAsBytes
        {
            get { return Token.ResolvedAsBytes; }
        }      

        private TokenString Token { get; set; }
    }
}
