using System;
using System.Text;

namespace PdfReader
{
    public class PdfIdentifier : PdfObject
    {
        public PdfIdentifier(PdfObject parent, ParseIdentifier name)
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

        public ParseIdentifier ParseIdentifier { get => ParseObject as ParseIdentifier; }
        public string Value { get => ParseIdentifier.Value; }
    }
}
