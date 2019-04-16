using System;

namespace PdfXenon.Standard
{
    public class PdfDateTime : PdfObject
    {
        public PdfDateTime(PdfDocument doc, ParseString parse)
            : base(doc)
        {
            DateTime = parse.ValueAsDateTime;
        }

        public override string ToString()
        {
            return DateTime.ToString();
        }

        public DateTime DateTime { get; private set; }
    }
}
