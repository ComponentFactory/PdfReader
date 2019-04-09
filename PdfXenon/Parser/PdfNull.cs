using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNull : PdfObject
    {
        public PdfNull(TokenKeyword token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"PdfNull";
        }

        public override long Position { get => Token.Position; }

        private TokenKeyword Token { get; set; }
    }
}
