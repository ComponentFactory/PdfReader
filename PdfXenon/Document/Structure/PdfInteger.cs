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
            return Value.ToString();
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ParseInteger ParseInteger { get => ParseObject as ParseInteger; }
        public int Value { get => ParseInteger.Value; }
    }
}
