using System;

namespace PdfXenon.Standard
{
    public class PdfPage : PdfPageInherit
    {
        private PdfDictionary _resources;
        private PdfRectangle _mediaBox;
        private PdfContents _contents;

        public PdfPage(PdfDocument doc, PdfPages parent, ParseDictionary parse)
            : base(doc, parent, parse)
        {
        }

        public override string ToString()
        {
            return $"PdfPage\n{base.ToString()}";
        }

        public PdfPages Parent { get => (Inherit as PdfPages); }

        public PdfDictionary Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new PdfDictionary(Doc, InheritableMandatoryValue<ParseDictionary>("Resources"));

                return _resources;
            }
        }

        public PdfRectangle MediaBox
        {
            get
            {
                if (_mediaBox == null)
                    _mediaBox = new PdfRectangle(Doc, InheritableMandatoryValue<ParseArray>("MediaBox"));

                return _mediaBox;
            }
        }

        public PdfContents Contents
        {
            get
            {
                if (_contents == null)
                    _contents = new PdfContents(Doc, OptionalValue<ParseObject>("Contents"));

                return _contents;
            }
        }
    }
}
