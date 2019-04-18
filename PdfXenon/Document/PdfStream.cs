using System;

namespace PdfXenon.Standard
{
    public class PdfStream : PdfObject
    {
        private ParseStream _stream;

        public PdfStream(PdfObject parent, ParseStream stream)
            : base(parent, stream)
        {
            _stream = stream;
        }

        public bool HasFilter { get => _stream.HasFilter; }
        public byte[] ContentAsBytes { get => _stream.ContentAsBytes; }
        public string ContentAsString { get => _stream.ContentAsString; }
    }
}
