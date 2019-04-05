using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObject : PdfObject
    {
        public PdfIndirectObject(TokenNumeric id, TokenNumeric gen, PdfObject obj)
        {
            TokenId = id;
            TokenGen = gen;
            Obj = obj;
        }

        public int Id { get => TokenId.Integer.Value; }
        public int Gen { get => TokenGen.Integer.Value; }
        public PdfObject Obj { get; private set; }
        public override long Position { get => TokenId.Position; }

        private TokenNumeric TokenId { get; set; }
        private TokenNumeric TokenGen { get; set; }
    }
}
