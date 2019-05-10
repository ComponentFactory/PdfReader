using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfContents : PdfObject
    {
        public PdfContents(PdfObject parent, PdfObject obj)
            : base(parent)
        {
            Streams = new List<PdfStream>();
            ResolveToStreams(obj);
        }

        public List<PdfStream> Streams { get; private set; }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public PdfContentsParser CreateParser()
        {
            return new PdfContentsParser(this);
        }

        private void ResolveToStreams(PdfObject obj)
        {
            if (obj is PdfStream stream)
                Streams.Add(stream);
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
