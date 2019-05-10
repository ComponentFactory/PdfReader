using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIdentifier : PdfObject
    {
        public PdfIdentifier(PdfObject parent, ParseIdentifier name)
            : base(parent, name)
        {
        }

        public ParseIdentifier ParseIdentifier { get => ParseObject as ParseIdentifier; }
        public string Value { get => ParseIdentifier.Value; }
    }
}
