using System;

namespace PdfXenon.Standard
{
    public abstract class PdfObject
    {
        public PdfObject(PdfDocument doc)
        {
            Doc = doc;
        }

        public PdfDocument Doc { get; private set; }
    }
}
