using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfBoolean : PdfObject
    {
        public PdfBoolean(TokenKeyword token)
        {
            Token = token;
        }

        public bool Value { get => Token.Keyword == PdfKeyword.True; }
        public override long Position { get => Token.Position; }

        private TokenKeyword Token { get; set; }
    }
}
