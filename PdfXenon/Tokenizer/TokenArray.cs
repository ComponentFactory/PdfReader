using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenArrayOpen : TokenBase
    {
        public TokenArrayOpen(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"Array: Open, Pos: {Position}";
        }
    }

    public class TokenArrayClose : TokenBase
    {
        public TokenArrayClose(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"Array: Close, Pos: {Position}";
        }
    }
}
