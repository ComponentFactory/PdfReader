using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNull : PdfObject
    {
        public PdfNull(PdfObject parent)
            : base(parent)
        {
        }

        public override string ToString()
        {
            return "null";
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
