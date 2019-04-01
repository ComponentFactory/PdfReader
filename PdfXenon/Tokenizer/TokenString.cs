using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class TokenString : TokenBase
    {
        public abstract string ActualString { get; }
    }
}
