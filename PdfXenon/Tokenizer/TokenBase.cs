using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class TokenBase
    {
        public TokenBase(long position)
        {
            Position = position;
        }

        public long Position { get; private set; }
    }
}
