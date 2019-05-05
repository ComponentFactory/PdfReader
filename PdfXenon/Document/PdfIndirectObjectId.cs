using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObjectId : PdfObject
    {
        private PdfIndirectObject _single;
        private Dictionary<int, PdfIndirectObject> _gens;

        public PdfIndirectObjectId(PdfObject parent, int id)
            : base(parent)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"PdfIndirectObjectId Id:{Id} Count:{_gens.Count}";
        }

        public int Id { get; private set; }

        public int Count
        {
            get
            {
                if (_gens == null)
                    return (_single == null) ? 0 : 1;

                return _gens.Count;
            }
        }

        public bool ContainsGen(int gen)
        {
            if (_gens == null)
            {
                if (_single == null)
                    return false;
                else
                    return (_single.Gen == gen);
            }

            return _gens.ContainsKey(gen);
        }

        public Dictionary<int, PdfIndirectObject>.KeyCollection Gens
        {
            get
            {
                if (_gens == null)
                {
                    var temp = new Dictionary<int, PdfIndirectObject>();

                    if (_single != null)
                        temp.Add(_single.Gen, _single);

                    return temp.Keys;
                }

                return _gens.Keys;
            }
        }

        public Dictionary<int, PdfIndirectObject>.ValueCollection Values
        {
            get
            {
                if (_gens == null)
                {
                    var temp = new Dictionary<int, PdfIndirectObject>();

                    if (_single != null)
                        temp.Add(_single.Gen, _single);

                    return temp.Values;
                }

                return _gens.Values;
            }
        }

        public Dictionary<int, PdfIndirectObject>.Enumerator GetEnumerator()
        {
            if (_gens == null)
            {
                var temp = new Dictionary<int, PdfIndirectObject>();

                if (_single != null)
                    temp.Add(_single.Gen, _single);

                return temp.GetEnumerator();
            }

            return _gens.GetEnumerator();
        }

        public PdfIndirectObject this[int gen]
        {
            get
            {
                if (_gens == null)
                {
                    if ((_single != null) && (_single.Gen == gen))
                        return _single;
                    else
                        return null;
                }

                return _gens[gen];
            }
        }

        public void ResolveAllReferences(PdfDocument document)
        {
            if (_gens == null)
            {
                if ((_single != null) && (_single.Child == null))
                    document.ResolveReference(_single);
            }
            else
            {
                foreach (PdfIndirectObject indirect in _gens.Values)
                    if (indirect.Child != null)
                        document.ResolveReference(indirect);
            }
        }

        public void ResolveAllReferences(Parser parser, PdfDocument document)
        {
            if (_gens == null)
            {
                if ((_single != null) && (_single.Child == null))
                    document.ResolveReference(parser, _single);
            }
            else
            {
                foreach(PdfIndirectObject indirect in _gens.Values)
                    if (indirect.Child != null)
                        document.ResolveReference(parser, indirect);
            }
        }

        public void AddXRef(TokenXRefEntry xref)
        {
            PdfIndirectObject indirect = new PdfIndirectObject(this, xref);

            if (_gens == null)
            {
                if (_single == null)
                {
                    // Cache the single object
                    _single = indirect;
                    return;
                }
                else
                {
                    // Convert from single entry to using a dictionary
                    _gens = new Dictionary<int, PdfIndirectObject> { { _single.Gen, _single } };
                    _single = null;
                }
            }

            if (_gens.ContainsKey(xref.Gen))
                throw new ApplicationException($"Indirect object with Id:{xref.Id} Gen:{xref.Gen} already exists.");

            _gens.Add(indirect.Gen, indirect);
        }
    }
}
