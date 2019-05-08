using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfStream : PdfObject
    {
        private PdfDictionary _dictionary;

        public PdfStream(PdfObject parent, ParseStream stream)
            : base(parent, stream)
        {
        }

        public override string ToString()
        {
            return $"PdfStream Bytes:{ValueAsBytes.Length}";
        }

        public override string ToDebug()
        {
            return $"(Stream Bytes:{ValueAsBytes.Length})";
        }

        public ParseStream ParseStream { get => ParseObject as ParseStream; }
        public bool HasFilter { get => ParseStream.HasFilter; }

        public PdfDictionary Dictionary
        {
            get
            {
                if (_dictionary == null)
                    _dictionary = new PdfDictionary(this, ParseStream.Dictionary);

                return _dictionary;
            }
        }

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
