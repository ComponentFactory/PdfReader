using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectGen : PdfObject
    {
        public PdfIndirectObjectGen(PdfObject parent, TokenXRefEntry xref)
            : base(parent)
        {
            Id = xref.Id;
            Gen = xref.Gen;
            Offset = xref.Offset;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public long Offset { get; private set; }
        public PdfObject Child { get; set; }
    }
}