using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjects : PdfObject
    {
        private Dictionary<int, PdfIndirectObjectId> _ids = new Dictionary<int, PdfIndirectObjectId>();

        public PdfIndirectObjects(PdfObject parent)
            : base(parent)
        {
        }

        public int Count { get => _ids.Count; }
        public bool ContainsId(int id) { return _ids.ContainsKey(id); }
        public Dictionary<int, PdfIndirectObjectId>.KeyCollection Ids { get => _ids.Keys; }
        public Dictionary<int, PdfIndirectObjectId>.ValueCollection Values { get => _ids.Values; }
        public Dictionary<int, PdfIndirectObjectId>.Enumerator GetEnumerator() => _ids.GetEnumerator();
        public PdfIndirectObjectId this[int id] { get => _ids[id]; }
        public PdfIndirectObject this[int id, int gen] { get =>_ids[id][gen]; }
        public PdfIndirectObject this[PdfObjectReference reference] { get => this[reference.Id, reference.Gen]; }

        public T OptionalValue<T>(PdfObjectReference reference) where T : PdfObject
        {
            PdfObject obj = Document.ResolveReference(reference.Id, reference.Gen);

            if (obj != null)
            {
                if (!(obj is T))
                    throw new ApplicationException($"Optional indirect object ({reference.Id},{reference.Gen}) incorrect type.");

                return (T)obj;
            }

            return null;
        }

        public T MandatoryValue<T>(PdfObjectReference reference) where T : PdfObject
        {
            PdfObject obj = Document.ResolveReference(reference.Id, reference.Gen);

            if ((obj == null) ||  !(obj is T))
                throw new ApplicationException($"Mandatory indirect object ({reference.Id},{reference.Gen}) missing or incorrect type.");

            return (T)obj;
        }

        public void ResolveAllReferences(PdfDocument document)
        {
            foreach (var id in Values)
                id.ResolveAllReferences(document);
        }

        public void AddXRef(TokenXRefEntry xref)
        {
            // If this is the first time we have encountered this id, then add it
            if (!_ids.TryGetValue(xref.Id, out PdfIndirectObjectId indirectId))
            {
                indirectId = new PdfIndirectObjectId(this);
                _ids.Add(xref.Id, indirectId);
            }

            indirectId.AddXRef(xref);
        }
    }
}
