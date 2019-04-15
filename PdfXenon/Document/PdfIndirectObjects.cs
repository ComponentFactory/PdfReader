using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjects
    {
        private Dictionary<int, PdfIndirectObjectId> _identifiers = new Dictionary<int, PdfIndirectObjectId>();

        public PdfIndirectObjects(PdfDocument doc)
        {
            Doc = doc; 
        }

        public PdfDocument Doc { get; private set; }

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
            PdfIndirectObjectGen gen = this[reference];
            if ((gen == null) || (gen.Child == null) || !(gen.Child is ParseDictionary))
                throw new ApplicationException("Pages indirect reference missing or not a dictionary.");

            return gen.Child as T;
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
                indirectId = new PdfIndirectObjectId();
                _identifiers.Add(xref.Id, indirectId);
            }

            indirectId.AddXRef(xref);
        }
    }
}
