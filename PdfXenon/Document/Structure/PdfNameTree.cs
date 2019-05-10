using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNameTree : PdfObject
    {
        private readonly bool _root;
        private PdfDictionary _dictionary;
        private List<PdfNameTree> _children;
        private Dictionary<string, PdfObject> _names;

        public PdfNameTree(PdfDictionary dictionary, bool root = true)
            : base(dictionary.Parent)
        {
            _dictionary = dictionary;
            _root = root;

            if (_root)
                Load();
            else
            {
                PdfArray limits = _dictionary.MandatoryValue<PdfArray>("Limits");
                LimitMin = ((PdfString)limits.Objects[0]).Value;
                LimitMax = ((PdfString)limits.Objects[1]).Value;
            }
        }

        public string LimitMin { get; private set; }
        public string LimitMax { get; private set; }

        public PdfObject this[string name]
        {
            get
            {
                PdfObject ret = null;

                if ((string.Compare(name, LimitMin) >= 0) && (string.Compare(name, LimitMax) <= 0))
                {
                    if ((_names == null) && (_children == null))
                        Load();

                    if (_names != null)
                        _names.TryGetValue(name, out ret);
                    else
                    {
                        // Linear search, could improve perf by using a binary search
                        foreach (PdfNameTree child in _children)
                        {
                            if ((string.Compare(name, child.LimitMin) >= 0) && (string.Compare(name, child.LimitMax) <= 0))
                            {
                                ret = child[name];
                                break;
                            }
                        }
                    }
                }

                return ret;
            }
        }

        private void Load()
        {
            PdfArray kids = _dictionary.OptionalValue<PdfArray>("Kids");
            if (kids != null)
            {
                // Must load all the children as objects immediately, so we can then calculate the overall limits
                _children = new List<PdfNameTree>();
                foreach (PdfObjectReference reference in kids.Objects)
                    _children.Add(new PdfNameTree(Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference), false));

                // Only the root calculates the limits by examining the children
                if (_root)
                {
                    LimitMin = _children[0].LimitMin;
                    LimitMax = _children[_children.Count - 1].LimitMax;
                }
            }
            else
            {
                // Without 'Kids' the 'Names' is mandatory
                PdfArray array = _dictionary.MandatoryValue<PdfArray>("Names");

                _names = new Dictionary<string, PdfObject>();
                int count = array.Objects.Count;
                for (int i = 0; i < count; i += 2)
                {
                    PdfString key = (PdfString)array.Objects[i];
                    _names.Add(key.Value, array.Objects[i + 1]);

                    // Only the root calculates the limits by examining the enties
                    if (_root)
                    {
                        if (i == 0)
                            LimitMin = key.Value;
                        else if (i == (count - 1))
                            LimitMax = key.Value;
                    }
                }
            }
        }

    }
}
