using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNumeric : PdfObject
    {
        public PdfNumeric(TokenNumeric token)
        {
            Token = token;
        }

        public bool IsInteger { get => Token.IsInteger; }
        public int Integer { get => Token.Integer.Value; }
        public bool IsReal { get => Token.IsReal; }
        public double Real { get => Token.Real.Value; }
        public override long Position { get => Token.Position; }

        private TokenNumeric Token { get; set; }
    }
}
