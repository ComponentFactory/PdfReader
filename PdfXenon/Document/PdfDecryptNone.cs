using System;

namespace PdfXenon.Standard
{
    public class PdfDecryptNone : PdfDecrypt
    {
        public PdfDecryptNone(PdfObject parent)
            : base(parent)
        {
        }

        public override string DecodeString(PdfObject obj, string str)
        {
            return str;
        }

        public override byte[] DecodeBytes(PdfObject obj, byte[] bytes)
        {
            return bytes;
        }
    }
}
