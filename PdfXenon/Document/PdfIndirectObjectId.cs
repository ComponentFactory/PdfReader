using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectId : PdfObject
    {
        private Dictionary<int, PdfIndirectObject> _gens = new Dictionary<int, PdfIndirectObject>();

        public PdfIndirectObjectId(PdfObject parent)
            : base(parent)
        {
        }

        public int Count { get => _gens.Count; }
        public bool ContainsGen(int gen) { return _gens.ContainsKey(gen); }
        public Dictionary<int, PdfIndirectObject>.KeyCollection Gens { get => _gens.Keys; }
        public Dictionary<int, PdfIndirectObject>.ValueCollection Values { get => _gens.Values; }
        public Dictionary<int, PdfIndirectObject>.Enumerator GetEnumerator() => _gens.GetEnumerator();
        public PdfIndirectObject this[int gen] { get => _gens[gen]; }

        public void AddXRef(TokenXRefEntry xref)
        {
            if (_gens.ContainsKey(xref.Gen))
                throw new ApplicationException($"Indirect object with Id:{xref.Id} Gen:{xref.Gen} already exists, at position {xref.Position}.");

            _gens.Add(xref.Gen, new PdfIndirectObject(this, xref));
        }
    }
}
