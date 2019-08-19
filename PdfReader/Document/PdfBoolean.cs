using System;
using System.Text;

namespace PdfReader
{
    public class PdfBoolean : PdfObject
    {
        public PdfBoolean(PdfObject parent, ParseBoolean boolean)
            : base(parent, boolean)
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

        public ParseBoolean ParseBoolean { get => ParseObject as ParseBoolean; }
        public bool Value { get => ParseBoolean.Value; }
    }
}
