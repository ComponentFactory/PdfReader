using System;

namespace PdfXenon.Standard
{
    public class PdfCatalog : PdfDictionary
    {
        private PdfPagesTree _pages;

        public PdfCatalog(PdfDocument doc, ParseDictionary dictionary)
            : base(doc, dictionary)
        {
        }

        public override string ToString()
        {
            return $"PdfCatalog\n{base.ToString()}";
        }

        public PdfPagesTree Pages
        {
            get
            {
                if (_pages == null)
                    _pages = new PdfPagesTree(Doc, this, Doc.IndirectObjects.MandatoryValue<ParseDictionary>(MandatoryValue<ParseObjectReference>("Pages")));

                return _pages;
            }
        }
    }
}
