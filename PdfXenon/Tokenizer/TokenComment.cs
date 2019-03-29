using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenComment : TokenBase
    {
        public TokenComment(string comment)
        {
            Comment = comment;
        }

        public override string ToString()
        {
            return $"Comment:{Comment}";
        }

        public string Comment { get; private set; }
    }
}
