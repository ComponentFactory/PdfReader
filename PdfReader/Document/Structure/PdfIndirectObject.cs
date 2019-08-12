using System;
using System.Collections.Generic;
using System.Text;

namespace PdfReader
{
    public class PdfIndirectObject : PdfObject
    {
        public PdfIndirectObject(PdfObject parent, TokenXRefEntry xref)
            : base(parent)
        {
            Id = xref.Id;
            Gen = xref.Gen;
            Offset = xref.Offset;
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public long Offset { get; private set; }
        public PdfObject Child { get; set; }
    }
}