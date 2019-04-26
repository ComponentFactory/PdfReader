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

        public PdfContentsParser CreateParser()
        {
            return new PdfContentsParser(this, _streams);
        }

        private void ResolveToStreams(PdfObject obj)
        {
            if (obj is PdfStream)
                _streams.Add(obj as PdfStream);
            else if (obj is PdfObjectReference)
            {
                ResolveToStreams(Document.ResolveReference(obj as PdfObjectReference));
            }
            else if (obj is PdfArray)
            {
                PdfArray array = (PdfArray)obj;
                foreach (PdfObject entry in array.Objects)
                    ResolveToStreams(entry);
            }
        }
    }
}
