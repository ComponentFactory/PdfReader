using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfCatalog : PdfDictionary
    {
        private List<PdfPage> _pages;
        private PdfNumberTree _pageLabels;
        private PdfOutlineLevel _outlineRoot;
        private PdfStructTreeRoot _structTreeRoot;

        public PdfCatalog(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfName Version { get => OptionalValue<PdfName>("Version"); }

        public List<PdfPage> Pages
        {
            get
            {
                if (_pages == null)
                {
                    PdfDictionary dictionary = MandatoryValueRef<PdfDictionary>("Pages");
                    _pages = new List<PdfPage>();

                    // Build all the objects in the page tree, we only do this to get the list of pages
                    PdfPages rootPage = new PdfPages(this, dictionary.ParseObject as ParseDictionary);
                    rootPage.CreatePages(_pages);
                }

                return _pages;
            }
        }

        public PdfNumberTree PageLabels
        {
            get
            {
                if (_pageLabels == null)
                {
                    PdfDictionary dictionary = OptionalValueRef<PdfDictionary>("PageLabels");
                    if (dictionary != null)
                        _pageLabels = new PdfNumberTree(this, dictionary);
                }

                return _pageLabels;
            }
        }

        public PdfDictionary Names { get => OptionalValueRef<PdfDictionary>("Names"); }
        public PdfDictionary Dests { get => OptionalValueRef<PdfDictionary>("Dests"); }
        public PdfDictionary ViewerPreferences { get => OptionalValueRef<PdfDictionary>("ViewerPreferences"); }
        public PdfName PageLayout { get => OptionalValueRef<PdfName>("PageLayout"); }
        public PdfName PageMode { get => OptionalValueRef<PdfName>("PageMode"); }

        public PdfOutlineLevel Outlines
        {
            get
            {
                if (_outlineRoot == null)
                {
                    PdfDictionary dictionary = OptionalValueRef<PdfDictionary>("Outlines");
                    if (dictionary != null)
                        _outlineRoot = new PdfOutlineLevel(this, dictionary);
                }

                return _outlineRoot;
            }
        }

        public PdfArray Threads { get => OptionalValueRef<PdfArray>("Threads"); }
        public PdfObject OpenAction { get => OptionalValueRef<PdfObject>("OpenAction"); }
        public PdfDictionary AA { get => OptionalValueRef<PdfDictionary>("AA"); }
        public PdfDictionary URI { get => OptionalValueRef<PdfDictionary>("URI"); }
        public PdfDictionary AcroForm { get => OptionalValueRef<PdfDictionary>("AcroForm"); }
        public PdfStream Metadata { get => OptionalValueRef<PdfStream>("Metadata"); }

        public PdfStructTreeRoot StructTreeRoot
        {
            get
            {
                if (_structTreeRoot == null)
                {
                    PdfDictionary dictionary = OptionalValueRef<PdfDictionary>("StructTreeRoot");
                    if (dictionary != null)
                        _structTreeRoot = new PdfStructTreeRoot(this, dictionary.ParseDictionary);
                }

                return _structTreeRoot;
            }
        }
    }
}
