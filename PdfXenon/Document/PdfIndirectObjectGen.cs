using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectGen : PdfObject
    {
        public PdfIndirectObjectGen(PdfDocument doc, TokenXRefEntry xref)
            : base(doc)
        {
            Id = xref.Id;
            Gen = xref.Gen;
            Offset = xref.Offset;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public int Offset { get; private set; }
        public ParseObject Child { get; set; }
    }
}