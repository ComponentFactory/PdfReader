using System;

namespace PdfXenon.Standard
{
    public class PdfInteger : PdfObject
    {
        private ParseInteger _integer;

        public PdfInteger(PdfObject parent, ParseInteger integer)
            : base(parent, integer)
        {
            _integer = integer;
        }

        public int Value { get => _integer.Value; }
    }
}
