using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenName : TokenBase
    {
        public TokenName(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Name:{Name}";
        }

        public string Name { get; private set; }
    }
}
