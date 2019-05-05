using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfStream : PdfObject
    {
        public PdfStream(PdfObject parent, ParseStream stream)
            : base(parent, stream)
        {
        }

        public override string ToString()
        {
            return $"PdfStream Bytes:{ValueAsBytes.Length}";
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
