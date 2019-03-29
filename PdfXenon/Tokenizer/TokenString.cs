using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenString : TokenBase
    {
        public TokenString(string str, bool isHex)
        {
            HexString = str;
            IsHex = isHex;
        }

        public override string ToString()
        {
            return $"String: Len:{HexString.Length} Hex:{IsHex}";
        }

        public bool IsHex { get; private set; }
        public string HexString { get; private set; }
    }
}
