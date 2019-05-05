using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNull : PdfObject
    {
        public PdfNull(PdfObject parent)
            : base(parent)
        {
        }

        public override string ToString()
        {
            return $"PdfNull";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string output = "(null)";
            sb.Append(output);
            return indent + output.Length;
        }
    }
}
