using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfString : PdfObject
    {
        public PdfString(TokenHexString token)
        {
            Token = token;
        }

        public PdfString(TokenLiteralString token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"PdfString: {String}";
        }

        public string String
        {
            get
            {
                TokenLiteralString literal = Token as TokenLiteralString;
                if (literal != null)
                    return literal.ActualString;
                else
                {
                    TokenHexString hex = Token as TokenHexString;
                    return hex.ActualString;
                }
            }
        }

        public override long Position { get => Token.Position; }

        private TokenBase Token { get; set; }
    }
}
