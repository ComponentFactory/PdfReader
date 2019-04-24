using System;

namespace PdfXenon.Standard
{
    public class PdfInteger : PdfObject
    {
        public PdfInteger(PdfObject parent, ParseInteger integer)
            : base(parent, integer)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public ParseInteger ParseInteger { get => ParseObject as ParseInteger; }
        public int Value { get => ParseInteger.Value; }
    }
}
