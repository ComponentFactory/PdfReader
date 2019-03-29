using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenError : TokenBase
    {
        public TokenError(long position, string message)
        {
            Position = position;
            Message = message;
        }

        public override string ToString()
        {
            return $"Error:{Message} Pos:{Position}";
        }

        public long Position { get; private set; }
        public string Message { get; private set; }
    }
}
