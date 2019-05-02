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

        public override int Output(StringBuilder sb, int indent)
        {
            string output = "(null)";
            sb.Append(output);
            return indent + output.Length;
        }
    }
}
