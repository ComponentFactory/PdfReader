using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectGen
    {
        private TokenXRefEntry _xRef;

        public PdfIndirectObjectGen(TokenXRefEntry xref)
        {
            _xRef = xref;
        }

        public int Id { get => _xRef.Id; }
        public int Gen { get => _xRef.Gen; }
        public int Offset { get => _xRef.Offset; }
        public ParseObject Child { get; set; }
    }
}