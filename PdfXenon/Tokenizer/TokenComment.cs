using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenComment : TokenBase
    {
        public TokenComment(long position, string comment)
            : base(position)
        {
            Comment = comment;
        }

        public override string ToString()
        {
            return $"Comment: {Comment}, Pos: {Position}";
        }

        public string Comment { get; private set; }
    }
}
