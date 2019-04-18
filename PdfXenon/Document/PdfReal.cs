using System;

namespace PdfXenon.Standard
{
    public class PdfReal : PdfObject
    {
        private ParseReal _real;

        public PdfReal(PdfObject parent, ParseReal real)
            : base(parent, real)
        {
            _real = real;
        }

        public float Value { get => _real.Value; }
    }
}
