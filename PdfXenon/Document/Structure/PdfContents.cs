using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfContents : PdfObject
    {
        private List<PdfStream> _streams = new List<PdfStream>();

        public PdfContents(PdfObject parent, PdfObject obj)
            : base(parent)
        {
            ResolveToStreams(obj);
        }

        public override string ToString()
        {
            return $"PdfContents Streams:{_streams.Count}";
        }

        public override string ToDebug()
        {
            return $"(Contents Streams:{_streams.Count})";
        }

        public PdfContentsParser CreateParser()
        {
            return new PdfContentsParser(this, _streams);
        }

        private void ResolveToStreams(PdfObject obj)
        {
            if (obj is PdfStream stream)
                _streams.Add(stream);
            else if (obj is PdfObjectReference reference)
                ResolveToStreams(Document.ResolveReference(reference));
            else if (obj is PdfArray array)
            {
                foreach (PdfObject entry in array.Objects)
                    ResolveToStreams(entry);
            }
        }
    }
}
