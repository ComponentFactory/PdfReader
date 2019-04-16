using System;

namespace PdfXenon.Standard
{
    public class PdfDateTime : PdfObject
    {
        private DateTime _dateTime;

        public PdfDateTime(PdfDocument doc, ParseString parse)
            : base(doc)
        {
            // Todo parse the string
        }

        public override string ToString()
        {
            return _dateTime.ToString();
        }

        public DateTime Value { get; private set; }
    }
}
