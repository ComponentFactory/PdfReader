using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfObjectReference : PdfObject
    {
        public PdfObjectReference(TokenNumeric id, TokenNumeric gen)
        {
            TokenId = id;
            TokenGen = gen;
        }

        public override string ToString()
        {
            return $"PdfObjectReference: {Id} {Gen}";
        }

        public int Id { get => TokenId.Integer.Value; }
        public int Gen { get => TokenGen.Integer.Value; }
        public override long Position { get => TokenId.Position; }

        private TokenNumeric TokenId { get; set; }
        private TokenNumeric TokenGen { get; set; }
    }
}
