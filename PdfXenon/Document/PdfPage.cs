using System;

namespace PdfXenon.Standard
{
    public class PdfPage : PdfPageInherit
    {
        public PdfPage(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfDictionary Resources { get => InheritableMandatoryValue<PdfDictionary>("Resources"); }
        public PdfRectangle MediaBox { get => InheritableMandatoryValue<PdfRectangle>("MediaBox"); }
        public PdfContents Contents { get => InheritableMandatoryValue<PdfContents>("Contents"); }
    }
}
