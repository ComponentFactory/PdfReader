using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfInfo : PdfDictionary
    {
        public PdfInfo(PdfObject parent, ParseDictionary parse)
            : base(parent, parse)
        {
        }

        public PdfString Title { get => OptionalValue<PdfString>("Title"); }
        public PdfString Author { get => OptionalValue<PdfString>("Author"); }
        public PdfString Subject { get => OptionalValue<PdfString>("Subject"); }
        public PdfString Keywords { get => OptionalValue<PdfString>("Keywords"); }
        public PdfString Creator { get => OptionalValue<PdfString>("Creator"); }
        public PdfString Producer { get => OptionalValue<PdfString>("Producer"); }
        public PdfDateTime CreationDate { get => OptionalDateTime("CreationDate"); }
        public PdfDateTime ModDate { get => OptionalDateTime("ModDate"); }
        public PdfName Trapped { get => OptionalValue<PdfName>("Trapped"); }
    }
}
