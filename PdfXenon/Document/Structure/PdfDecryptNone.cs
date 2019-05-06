using System;
using System.Text;

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

        public override string DecodeStream(PdfStream stream)
        {
            return stream.ParseStream.Value;
        }

        public override byte[] DecodeStreamAsBytes(PdfStream stream)
        {
            return stream.ParseStream.ValueAsBytes;
        }
    }
}
