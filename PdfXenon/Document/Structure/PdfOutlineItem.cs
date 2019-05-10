using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfOutlineItem : PdfOutlineLevel
    {
        private PdfDictionary _dictionary;

        public PdfOutlineItem(PdfObject parent, PdfDictionary dictionary)
            : base(parent, dictionary)
        {
            _dictionary = dictionary;
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public PdfString Title { get => _dictionary.MandatoryValueRef<PdfString>("Title"); }
        public PdfObject Dest { get => _dictionary.OptionalValueRef<PdfObject>("Dest"); }
        public PdfDictionary A { get => _dictionary.OptionalValueRef<PdfDictionary>("A"); }
        public PdfDictionary SE { get => _dictionary.OptionalValueRef<PdfDictionary>("SE"); }
        public PdfArray C { get => _dictionary.OptionalValueRef<PdfArray>("C"); }
        public PdfInteger F { get => _dictionary.OptionalValueRef<PdfInteger>("F"); }
    }
}
