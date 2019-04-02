using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenName : TokenBase
    {
        public TokenName(long position, string name)
            : base(position)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Pos: {Position}";
        }

        public string Name { get; private set; }
    }
}
