using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfBoolean : PdfObject
    {
        public PdfBoolean(PdfObject parent, ParseBoolean boolean)
            : base(parent, boolean)
        {
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value.ToString();
            sb.Append(output);
            return indent + output.Length;
        }

        public ParseBoolean ParseBoolean { get => ParseObject as ParseBoolean; }
        public bool Value { get => ParseBoolean.Value; }
    }
}
