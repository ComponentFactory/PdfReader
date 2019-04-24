using System;

namespace PdfXenon.Standard
{
    public class PdfDateTime : PdfString
    {
        public PdfDateTime(PdfObject parent, PdfString str)
            : base(parent, str.ParseObject as ParseString)
        {
            DateTime = str.ValueAsDateTime;
        }

        public override string ToString()
        {
            return DateTime.ToString();
        }

        public DateTime DateTime { get; private set; }
    }
}
