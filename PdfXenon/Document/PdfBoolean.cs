using System;

namespace PdfXenon.Standard
{
    public class PdfBoolean : PdfObject
    {
        public PdfBoolean(PdfObject parent, ParseBoolean boolean)
            : base(parent, boolean)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public ParseBoolean ParseBoolean { get => ParseObject as ParseBoolean; }
        public bool Value { get => ParseBoolean.Value; }
    }
}
