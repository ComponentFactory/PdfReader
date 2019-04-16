using System;

namespace PdfXenon.Standard
{
    public class PdfPage : PdfPageInherit
    {
        private PdfDictionary _resources;
        private PdfRectangle _mediaBox;
        private PdfContent _contents;

        public PdfPage(PdfDocument doc, PdfPages parent, ParseDictionary dictionary)
            : base(doc, parent, dictionary)
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
                    _resources = new PdfDictionary(Doc, MandatoryInheritableValue<ParseDictionary>("Resources"));

                return _resources;
            }
        }

        public PdfRectangle MediaBox
        {
            get
            {
                if (_mediaBox == null)
                    _mediaBox = new PdfRectangle(Doc, MandatoryInheritableValue<ParseArray>("MediaBox"));

                return _mediaBox;
            }
        }

        public PdfContent Contents
        {
            get
            {
                if (_contents == null)
                    _contents = new PdfContent(Doc, OptionalValue<ParseObject>("Contents"));

                return _contents;
            }
        }
    }
}
