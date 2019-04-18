using System;

namespace PdfXenon.Standard
{
    public class PdfName : PdfObject
    {
        private ParseName _name;

        public PdfName(PdfObject parent, ParseName name)
            : base(parent, name)
        {
            _name = name;
        }

        public string Value { get => _name.Value; }
    }
}
