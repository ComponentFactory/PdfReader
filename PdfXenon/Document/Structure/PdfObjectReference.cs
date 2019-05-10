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

        public ParseObjectReference ParseObjectReference { get => ParseObject as ParseObjectReference; }
        public int Id { get => ParseObjectReference.Id; }
        public int Gen { get => ParseObjectReference.Gen; }
    }
}
