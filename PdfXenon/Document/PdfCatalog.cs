using System;

namespace PdfXenon.Standard
{
    public class PdfCatalog : PdfDictionary
    {
        private PdfPagesTree _pages;

        public PdfCatalog(PdfDocument document, ParseDictionary dictionary)
            : base(dictionary)
        {
            Document = document;
        }

        public PdfDocument Document { get; private set; }

        public PdfPagesTree Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new PdfPagesTree(this, Document.IndirectObjects.MandatoryValue<ParseDictionary>(MandatoryValue<ParseObjectReference>("Pages")));

                return _pages;
            }
        }
    }
}
