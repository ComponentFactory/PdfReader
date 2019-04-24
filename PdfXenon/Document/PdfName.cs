using System;

namespace PdfXenon.Standard
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

        public ParseName ParseName { get => ParseObject as ParseName; }
        public string Value { get => ParseName.Value; }
    }
}
