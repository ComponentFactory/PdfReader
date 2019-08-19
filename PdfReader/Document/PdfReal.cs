using System;
using System.Text;

namespace PdfReader
{
    public class PdfReal : PdfObject
    {
        public PdfReal(PdfObject parent, ParseReal real)
            : base(parent, real)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ParseReal ParseReal { get => ParseObject as ParseReal; }
        public float Value { get => ParseReal.Value; }
    }
}
