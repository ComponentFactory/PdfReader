using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfCatalog : PdfDictionary
    {
        private PdfPages _pageTree;
        private List<PdfPage> _pages;

        public PdfCatalog(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public List<PdfPage> Pages
        {
            get
            {
                if (_pages == null)
                {
                    // Accessing the root pages will cause it to generate the pages collection
                    var temp = RootPages;
                }

                return _pages;
            }
        }

        public PdfPages RootPages
        {
            get
            {
                if (_pageTree == null)
                {
                    // Catalog has a mandatory 'Pages' entry that is a reference to the page tree root
                    PdfObjectReference reference = MandatoryValue<PdfObjectReference>("Pages");
                    PdfDictionary dictionary = Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference);

                    // Build all the objects in the page tree
                    _pageTree = new PdfPages(this, dictionary.ParseObject as ParseDictionary);
                    _pages = new List<PdfPage>();
                    _pageTree.CreatePages(_pages);
                }

                return _pageTree;
            }
        }
    }
}
