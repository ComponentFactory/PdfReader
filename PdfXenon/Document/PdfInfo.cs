using System;

namespace PdfXenon.Standard
{
    public class PdfInfo : PdfDictionary
    {
        public PdfInfo(PdfDocument doc, ParseDictionary parse)
            : base(doc, parse)
        {
        }

        public override string ToString()
        {
            return $"PdfInfo\n{base.ToString()}";
        }

        public ParseString Title { get => OptionalValue<ParseString>("Title"); }
        public ParseString Author { get => OptionalValue<ParseString>("Author"); }
        public ParseString Subject { get => OptionalValue<ParseString>("Subject"); }
        public ParseString Keywords { get => OptionalValue<ParseString>("Keywords"); }
        public ParseString Creator { get => OptionalValue<ParseString>("Creator"); }
        public ParseString Producer { get => OptionalValue<ParseString>("Producer"); }
        public PdfDateTime CreationDate { get => OptionalDateTime("CreationDate"); }
        public PdfDateTime ModDate { get => OptionalDateTime("ModDate"); }
    }
}
