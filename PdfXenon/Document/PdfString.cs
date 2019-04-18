using System;

namespace PdfXenon.Standard
{
    public class PdfString : PdfObject
    {
        private ParseString _string;

        public PdfString(PdfObject parent, ParseString str)
            : base(parent, str)
        {
            _string = str;
        }

        public string Value { get => _string.Value; }
        public byte[] ValueAsBytes { get => _string.ValueAsBytes; }
        public DateTime ValueAsDateTime { get => _string.ValueAsDateTime; }
    }
}
