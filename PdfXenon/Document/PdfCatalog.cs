using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfCatalog : PdfDictionary
    {
        private PdfPages _pageTree;
        private List<PdfPage> _pages;

        public PdfCatalog(PdfDocument doc, ParseDictionary parse)
            : base(doc, parse)
        {
        }

        public override string ToString()
        {
            return $"PdfCatalog\n{base.ToString()}";
        }

        public List<PdfPage> Pages
        {
            get
            {
                if (_pages == null)
                {
                    // Accessing the PageTree will cause it to generate the pages collection
                    var temp = PageTree;
                }

                return _pages;
            }
        }

        public PdfPages PageTree
        {
            get
            {
                if (_pageTree == null)
                {
                    // Catalog has a mandatory 'Pages' entry that is a reference to the page tree root
                    ParseObjectReference reference = MandatoryValue<ParseObjectReference>("Pages");
                    ParseDictionary dictionary = Doc.IndirectObjects.MandatoryValue<ParseDictionary>(reference);

                    // Build all the objects in the page tree
                    _pageTree = new PdfPages(Doc, null, dictionary);
                    _pages = new List<PdfPage>();
                    _pageTree.CreatePages(_pages);
                }

                return _pageTree;
            }
        }
    }
}
