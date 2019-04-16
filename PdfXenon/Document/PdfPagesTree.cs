using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfPagesTree : PdfPages
    {
        private List<PdfPage> _pages;

        public PdfPagesTree(PdfDocument doc, PdfCatalog catalog, ParseDictionary dictionary)
            : base(doc, null, dictionary)
        {
            Catalog = catalog;
        }

        public PdfCatalog Catalog { get; private set; }
        public PdfPage this[int index] { get { return Pages[index]; } }

        private List<PdfPage> Pages
        {
            get
            {
                if (_pages == null)
                {
                    // Create the page tree hierarchy and accumulate the 
                    _pages = new List<PdfPage>();
                    CreatePages(_pages);
                }

                return _pages;
            }
        }
    }
}
