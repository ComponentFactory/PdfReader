using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfReal : PdfObject
    {
        public PdfReal(PdfObject parent, ParseReal real)
            : base(parent, real)
        {
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value.ToString();
            sb.Append(output);
            return indent + output.Length;
        }

        public ParseReal ParseReal { get => ParseObject as ParseReal; }
        public float Value { get => ParseReal.Value; }
    }
}
