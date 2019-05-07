using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfObjectReference : PdfObject
    {
        public PdfObjectReference(PdfObject parent, ParseObjectReference reference)
            : base(parent, reference)
        {
        }

        public override string ToString()
        {
            return $"PdfObjectReference Id:{Id} Gen:{Gen}";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string output = $"{Id} {Gen} R";
            sb.Append(output);
            return indent + output.Length;
        }

        public ParseObjectReference ParseObjectReference { get => ParseObject as ParseObjectReference; }
        public int Id { get => ParseObjectReference.Id; }
        public int Gen { get => ParseObjectReference.Gen; }
    }
}
