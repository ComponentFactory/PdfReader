using System;

namespace PdfXenon.Standard
{
    public class PdfInfo : PdfDictionary
    {
        private PdfPagesTree _pages;

        public PdfInfo(PdfDocument doc, ParseDictionary dictionary)
            : base(doc, dictionary)
        {
        }

        public override string ToString()
        {
            return $"PdfInfo\n{base.ToString()}";
        }

        public ParseString Title { get { return OptionalValue<ParseString>("Title"); } }
        public ParseString Author { get { return OptionalValue<ParseString>("Author"); } }
        public ParseString Subject { get { return OptionalValue<ParseString>("Subject"); } }
        public ParseString Keywords { get { return OptionalValue<ParseString>("Keywords"); } }
        public ParseString Creator { get { return OptionalValue<ParseString>("Creator"); } }
        public ParseString Producer { get { return OptionalValue<ParseString>("Producer"); } }

        public PdfDateTime CreationDate
        {
            get
            {
                ParseString parse = OptionalValue<ParseString>("CreationDate");
                if (parse != null)
                    return new PdfDateTime(Doc, parse);
                else
                    return null;
            }
        }

        public PdfDateTime ModDate
        {
            get
            {
                ParseString parse = OptionalValue<ParseString>("ModDate");
                if (parse != null)
                    return new PdfDateTime(Doc, parse);
                else
                    return null;
            }
        }
    }
}
