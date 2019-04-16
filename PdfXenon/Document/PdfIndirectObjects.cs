using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjects : PdfObject
    {
        private Dictionary<int, PdfIndirectObjectId> _ids = new Dictionary<int, PdfIndirectObjectId>();

        public PdfIndirectObjects(PdfDocument doc)
            : base(doc)
        {
        }

        public int Count { get => _ids.Count; }
        public bool ContainsId(int id) { return _ids.ContainsKey(id); }
        public Dictionary<int, PdfIndirectObjectId>.KeyCollection Ids { get => _ids.Keys; }
        public Dictionary<int, PdfIndirectObjectId>.ValueCollection Values { get => _ids.Values; }
        public Dictionary<int, PdfIndirectObjectId>.Enumerator GetEnumerator() => _ids.GetEnumerator();
        public PdfIndirectObjectId this[int id] { get => _ids[id]; }
        public PdfIndirectObjectGen this[int id, int gen] { get =>_ids[id][gen]; }
        public PdfIndirectObjectGen this[ParseObjectReference reference] { get => this[reference.Id, reference.Gen]; }

        public T MandatoryValue<T>(ParseObjectReference reference) where T : ParseObject
        {
            ParseObject obj = Doc.ResolveReference(reference.Id, reference.Gen);
            if ((obj == null) ||  !(obj is T))
                throw new ApplicationException($"Mandatory indirect object ({reference.Id},{reference.Gen}) missing or incorrect type at position {reference.Position}.");

            return (T)obj;
        }

        public void AddXRef(TokenXRefEntry xref)
        {
            // If this is the first time we have encountered this id, then add it
            if (!_ids.TryGetValue(xref.Id, out PdfIndirectObjectId indirectId))
            {
                indirectId = new PdfIndirectObjectId(Doc);
                _ids.Add(xref.Id, indirectId);
            }

            indirectId.AddXRef(xref);
        }
    }
}
