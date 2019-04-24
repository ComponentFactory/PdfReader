using System;

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
            return $"{Id} {Gen} R";
        }

        public ParseObjectReference ParseObjectReference { get => ParseObject as ParseObjectReference; }
        public int Id { get => ParseObjectReference.Id; }
        public int Gen { get => ParseObjectReference.Gen; }
    }
}
