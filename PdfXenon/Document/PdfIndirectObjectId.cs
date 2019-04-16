using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectId : PdfObject
    {
        private Dictionary<int, PdfIndirectObjectGen> _gens = new Dictionary<int, PdfIndirectObjectGen>();

        public PdfIndirectObjectId(PdfDocument doc)
            : base(doc)
        {
        }

        public int Count { get => _gens.Count; }
        public bool ContainsGen(int gen) { return _gens.ContainsKey(gen); }
        public Dictionary<int, PdfIndirectObjectGen>.KeyCollection Gens { get => _gens.Keys; }
        public Dictionary<int, PdfIndirectObjectGen>.ValueCollection Values { get => _gens.Values; }
        public Dictionary<int, PdfIndirectObjectGen>.Enumerator GetEnumerator() => _gens.GetEnumerator();
        public PdfIndirectObjectGen this[int gen] { get => _gens[gen]; }

        public void AddXRef(TokenXRefEntry xref)
        {
            if (_gens.ContainsKey(xref.Gen))
                throw new ApplicationException($"Indirect object with Id:{xref.Id} Gen:{xref.Gen} already exists, at position {xref.Position}.");

            _gens.Add(xref.Gen, new PdfIndirectObjectGen(Doc, xref));
        }
    }
}
