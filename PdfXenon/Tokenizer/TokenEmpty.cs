using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenEmpty : TokenBase
    {
        public TokenEmpty(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"(Empty), Pos: {Position}";
        }
    }
}
