using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectId
    {
        private Dictionary<int, PdfIndirectObjectGen> _generations = new Dictionary<int, PdfIndirectObjectGen>();

        public PdfIndirectObjectId(PdfIndirectObjects indirectObjects)
        {
            IndirectObjects = indirectObjects;
        }

        public PdfIndirectObjects IndirectObjects { get; private set; }

        public int Count { get => _generations.Count; }

        public bool ContainsGen(int gen)
        {
            return _generations.ContainsKey(gen);
        }

        public PdfIndirectObjectGen this[int gen]
        {
            get
            {
                _generations.TryGetValue(gen, out PdfIndirectObjectGen indirect);
                return indirect;
            }
        }

        public Dictionary<int, PdfIndirectObjectGen>.KeyCollection Gens
        {
            get { return _generations.Keys; }
        }

        public Dictionary<int, PdfIndirectObjectGen>.ValueCollection Values
        {
            get { return _generations.Values; }
        }

        public void AddXRef(TokenXRefEntry xref)
        {
            if (_generations.ContainsKey(xref.Gen))
                throw new ApplicationException($"Indirect object with Id:{xref.Id} Gen:{xref.Gen} already exists, at position {xref.Position}.");

            _generations.Add(xref.Gen, new PdfIndirectObjectGen(this, xref));
        }
    }
}
