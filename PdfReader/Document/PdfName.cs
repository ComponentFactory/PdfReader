using System;
using System.Text;

namespace PdfReader
{
    public class PdfName : PdfObject
    {
        public PdfName(PdfObject parent, ParseName name)
            : base(parent, name)
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

        public ParseName ParseName { get => ParseObject as ParseName; }
        public string Value { get => ParseName.Value; }
    }
}
