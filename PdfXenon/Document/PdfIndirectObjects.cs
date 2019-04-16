using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjects : PdfObject
    {
        private Dictionary<int, PdfIndirectObjectId> _identifiers = new Dictionary<int, PdfIndirectObjectId>();

        public PdfIndirectObjects(PdfDocument doc)
            : base(doc)
        {
        }

        public int Count { get => _identifiers.Count; }

        public bool ContainsId(int id)
        {
            return _identifiers.ContainsKey(id);
        }

        public PdfIndirectObjectId this[int id]
        {
            get
            {
                _identifiers.TryGetValue(id, out PdfIndirectObjectId indirectId);
                return indirectId;
            }
        }

        public PdfIndirectObjectGen this[int id, int gen]
        {
            get
            {
                if (_identifiers.TryGetValue(id, out PdfIndirectObjectId indirectId))
                    return indirectId[gen];
                else
                    return null;
            }
        }

        public PdfIndirectObjectGen this[ParseObjectReference reference]
        {
            get { return this[reference.Id, reference.Gen]; }
        }

        public T MandatoryValue<T>(ParseObjectReference reference) where T : ParseObject
        {
            ParseObject obj = Doc.ResolveReference(reference.Id, reference.Gen);
            if ((obj == null) ||  !(obj is T))
                throw new ApplicationException($"Mandatory indirect object ({reference.Id},{reference.Gen}) missing or incorrect type at position {reference.Position}.");

            return (T)obj;
        }

        public Dictionary<int, PdfIndirectObjectId>.KeyCollection Ids
        {
            get { return _identifiers.Keys; }
        }

        public Dictionary<int, PdfIndirectObjectId>.ValueCollection Values
        {
            get { return _identifiers.Values; }
        }

        public void AddXRef(TokenXRefEntry xref)
        {
            // If this is the first time we have encountered this id, then add it
            if (!_identifiers.TryGetValue(xref.Id, out PdfIndirectObjectId indirectId))
            {
                indirectId = new PdfIndirectObjectId(Doc);
                _identifiers.Add(xref.Id, indirectId);
            }

            indirectId.AddXRef(xref);
        }
    }
}
