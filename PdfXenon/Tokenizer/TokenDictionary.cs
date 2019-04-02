using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenDictionaryOpen : TokenBase
    {
        public TokenDictionaryOpen(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"Dictionary: Open, Pos: {Position}";
        }
    }

    public class TokenDictionaryClose : TokenBase
    {
        public TokenDictionaryClose(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"Dictionary: Close, Pos: {Position}";
        }
    }
}
