using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectGen
    {
        private TokenXRefEntry _xRef;
        private ParseObject _object;

        public PdfIndirectObjectGen(PdfIndirectObjectId indirectObjectId, TokenXRefEntry xref)
        {
            IndirectObjectId = indirectObjectId;
            _xRef = xref;
        }

        public PdfIndirectObjectId IndirectObjectId { get; private set; }
        public int Id { get => _xRef.Id; }
        public int Gen { get => _xRef.Gen; }
        public int Offset { get => _xRef.Offset; }
        public ParseObject Child { get; set; }
    }
}