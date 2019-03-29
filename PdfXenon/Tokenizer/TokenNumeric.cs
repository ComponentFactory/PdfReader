using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenNumeric : TokenBase
    {
        public TokenNumeric(int integer)
        {
            Integer = integer;
        }

        public TokenNumeric(double real)
        {
            Real = real;
        }

        public override string ToString()
        {
            if (Integer.HasValue)
                return $"Integer:{Integer.Value}";
            else if (Real.HasValue)
                return $"Real:{Integer.Value}";
            else
                return $"Numeric:(null)";
        }

        public int? Integer { get; set; }
        public double? Real { get; set; }
    }
}
