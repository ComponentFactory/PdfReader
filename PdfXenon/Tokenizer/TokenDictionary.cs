using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenDictionaryOpen : TokenBase
    {
        public override string ToString()
        {
            return "Dictionary Open";
        }
    }

    public class TokenDictionaryClose : TokenBase
    {
        public override string ToString()
        {
            return "Dictionary Close";
        }
    }
}
