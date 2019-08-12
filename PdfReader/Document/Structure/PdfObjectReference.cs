using System;
using System.Text;

namespace PdfReader
{
    public class PdfObjectReference : PdfObject
    {
        public PdfObjectReference(PdfObject parent, ParseObjectReference reference)
            : base(parent, reference)
        {
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ParseObjectReference ParseObjectReference { get => ParseObject as ParseObjectReference; }
        public int Id { get => ParseObjectReference.Id; }
        public int Gen { get => ParseObjectReference.Gen; }
    }
}
