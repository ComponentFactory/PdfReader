using System;

namespace PdfXenon.Standard
{
    public class PdfPage : PdfPageInherit
    {
        private PdfContents _contents;

        public PdfPage(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfDictionary Resources { get => InheritableMandatoryValue<PdfDictionary>("Resources"); }
        public PdfRectangle MediaBox { get => InheritableMandatoryValue<PdfRectangle>("MediaBox"); }

        public PdfContents Contents
        {
            get
            {
                if (_contents == null)
                {
                    PdfObject obj = InheritableMandatoryValue<PdfObject>("Contents");
                    _contents = new PdfContents(this, obj);
                }

                return _contents;
            }

        }
    }
}
