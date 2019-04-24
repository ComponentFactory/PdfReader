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
        public byte[] ContentAsBytes { get => ParseStream.ContentAsBytes; }
        public string ContentAsString { get => ParseStream.ContentAsString; }
    }
}
