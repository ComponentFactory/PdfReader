using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfName : PdfObject
    {
        public PdfName(PdfObject parent, ParseName name)
            : base(parent, name)
        {
        }

        public ParseName ParseName { get => ParseObject as ParseName; }
        public string Value { get => ParseName.Value; }
    }
}
