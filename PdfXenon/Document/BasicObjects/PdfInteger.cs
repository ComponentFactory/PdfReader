using System;
using System.Text;

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
            return $"PdfInteger {Value}";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string output = Value.ToString();
            sb.Append(output);
            return indent + output.Length;
        }

        public ParseInteger ParseInteger { get => ParseObject as ParseInteger; }
        public int Value { get => ParseInteger.Value; }
    }
}
