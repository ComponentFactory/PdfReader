using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenNumeric : TokenBase
    {
        public TokenNumeric(long position, int integer)
            : base(position)
        {
            Integer = integer;
        }

        public TokenNumeric(long position, double real)
            : base(position)
        {
            Real = real;
        }

        public override string ToString()
        {
            if (Integer.HasValue)
                return $"Integer: {Integer.Value}, Pos: {Position}";
            else if (Real.HasValue)
                return $"Real: {Real.Value}, Pos: {Position}";
            else
                return $"Numeric: (null), Pos: {Position}";
        }

        public int? Integer { get; set; }
        public double? Real { get; set; }

        public bool IsInteger { get => Integer.HasValue; }
        public bool IsReal { get => Real.HasValue; }
    }
}
