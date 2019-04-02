using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfName : PdfObject
    {
        public PdfName(TokenName token)
        {
            Token = token;
        }

        public string Name { get => Token.Name; }
        public override long Position { get => Token.Position; }

        private TokenName Token { get; set; }
    }
}
