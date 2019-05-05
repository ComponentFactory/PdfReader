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

        public override string ToString()
        {
            return $"PdfOutlineItem Title:{Title}";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);

            string output = Title.Value;
            sb.Append(output);
            sb.Append("\n");
            sb.Append(blank);
            base.ToDebug(sb, indent + 2);

            return indent;
        }

        public PdfString Title { get => _dictionary.MandatoryValueRef<PdfString>("Title"); }
        public PdfObject Dest { get => _dictionary.OptionalValueRef<PdfObject>("Dest"); }
        public PdfDictionary A { get => _dictionary.OptionalValueRef<PdfDictionary>("A"); }
        public PdfDictionary SE { get => _dictionary.OptionalValueRef<PdfDictionary>("SE"); }
        public PdfArray C { get => _dictionary.OptionalValueRef<PdfArray>("C"); }
        public PdfInteger F { get => _dictionary.OptionalValueRef<PdfInteger>("F"); }
    }
}
