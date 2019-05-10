using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfBoolean : PdfObject
    {
        public PdfBoolean(PdfObject parent, ParseBoolean boolean)
            : base(parent, boolean)
        {
        }

        public ParseBoolean ParseBoolean { get => ParseObject as ParseBoolean; }
        public bool Value { get => ParseBoolean.Value; }
    }
}
