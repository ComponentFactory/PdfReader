using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIdentifier : PdfObject
    {
        public PdfIdentifier(PdfObject parent, ParseIdentifier name)
            : base(parent, name)
        {
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value;
            sb.Append(output);
            return indent + output.Length;
        }

        public ParseIdentifier ParseIdentifier { get => ParseObject as ParseIdentifier; }
        public string Value { get => ParseIdentifier.Value; }
    }
}
