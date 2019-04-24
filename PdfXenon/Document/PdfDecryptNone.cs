using System;

namespace PdfXenon.Standard
{
    public class PdfDecryptNone : PdfDecrypt
    {
        public PdfDecryptNone(PdfObject parent)
            : base(parent)
        {
        }

        public override string DecodeString(PdfString obj)
        {
            return obj.ParseString.Value;
        }

        public override byte[] DecodeStringAsBytes(PdfString obj)
        {
            return obj.ParseString.ValueAsBytes;
        }
    }
}
