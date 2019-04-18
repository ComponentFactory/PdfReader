using System;

namespace PdfXenon.Standard
{
    public class PdfObjectReference : PdfObject
    {
        private ParseObjectReference _reference;

        public PdfObjectReference(PdfObject parent, ParseObjectReference reference)
            : base(parent, reference)
        {
            _reference = reference;
        }

        public int Id { get => _reference.Id; }
        public int Gen { get => _reference.Gen; }
    }
}
