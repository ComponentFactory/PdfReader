using System;

namespace PdfXenon.Standard
{
    public class PdfStream : PdfObject
    {
        public PdfStream(PdfObject parent, ParseStream stream)
            : base(parent, stream)
        {
        }

        public ParseStream ParseStream { get => ParseObject as ParseStream; }
        public bool HasFilter { get => ParseStream.HasFilter; }

        public string Value
        {
            get { return Decrypt.DecodeStream(this); }
        }

        public byte[] ValueAsBytes
        {
            get { return Decrypt.DecodeStreamAsBytes(this); }
        }
    }
}
