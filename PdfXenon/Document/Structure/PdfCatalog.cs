using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfCatalog : PdfDictionary
    {
        private List<PdfPage> _pages;
        private PdfPages _rootPage;
        private PdfNumberTree _pageLabels;
        private PdfOutlineLevel _outlineRoot;
        private PdfStructTreeRoot _structTreeRoot;

        public PdfCatalog(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public PdfName Version { get => OptionalValue<PdfName>("Version"); }

        public List<PdfPage> Pages
        {
            get
            {
                if (_rootPage == null)
                {
                    // Accessing the RootPage will cause the pages to be loaded
                    var temp = RootPage;
                }

                return _pages;
            }
        }

        public PdfPages RootPage
        {
            get
            {
                if (_rootPage == null)
                {
                    PdfDictionary dictionary = MandatoryValueRef<PdfDictionary>("Pages");

                    // Page tree construct the hierarchy so that inheritance of properties works correctly
                    _rootPage = new PdfPages(dictionary);

                    // Flatten the hierarchy into a list of pages, this is more useful for the user
                    _pages = new List<PdfPage>();
                    _rootPage.FindLeafPages(_pages);
                }

                return _rootPage;
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
                        _pageLabels = new PdfNumberTree(dictionary);
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

        public PdfDictionary MarkInfo { get => OptionalValueRef<PdfDictionary>("MarkInfo"); }
        public PdfString Lang { get => OptionalValueRef<PdfString>("Lang"); }
        public PdfDictionary SpiderInfo { get => OptionalValueRef<PdfDictionary>("SpiderInfo"); }
        public PdfArray OutputIntents { get => OptionalValueRef<PdfArray>("OutputIntents"); }
        public PdfDictionary PieceInfo { get => OptionalValueRef<PdfDictionary>("PieceInfo"); }
        public PdfDictionary OCProperties { get => OptionalValueRef<PdfDictionary>("OCProperties"); }
        public PdfDictionary Perms { get => OptionalValueRef<PdfDictionary>("Perms"); }
        public PdfDictionary Legal { get => OptionalValueRef<PdfDictionary>("Legal"); }
        public PdfArray Requirements { get => OptionalValueRef<PdfArray>("Requirements"); }
        public PdfDictionary Collection { get => OptionalValueRef<PdfDictionary>("Collection"); }
        public PdfBoolean NeedsRendering { get => OptionalValueRef<PdfBoolean>("NeedsRendering"); }
    }
}
