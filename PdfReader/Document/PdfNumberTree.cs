using System;
using System.Collections.Generic;
using System.Text;

namespace PdfReader
{
    public class PdfNumberTree : PdfObject
    {
        private readonly bool _root;
        private PdfDictionary _dictionary;
        private List<PdfNumberTree> _children;
        private Dictionary<int, PdfObject> _nums;

        public PdfNumberTree(PdfDictionary dictionary, bool root = true)
            : base(dictionary.Parent)
        {
            _dictionary = dictionary;
            _root = root;

            if (_root)
                Load();
            else
            {
                PdfArray limits = _dictionary.MandatoryValue<PdfArray>("Limits");
                LimitMin = ((PdfInteger)limits.Objects[0]).Value;
                LimitMax = ((PdfInteger)limits.Objects[1]).Value;
            }
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public int LimitMin { get; private set; }
        public int LimitMax { get; private set; }

        public PdfObject this[int number]
        {
            get
            {
                PdfObject ret = null;

                if ((number >= LimitMin) && (number <= LimitMax))
                {
                    if ((_nums == null) && (_children == null))
                        Load();

                    if (_nums != null)
                        _nums.TryGetValue(number, out ret);
                    else
                    {
                        // Linear search, could improve perf by using a binary search
                        foreach(PdfNumberTree child in _children)
                        {
                            if ((number >= child.LimitMin) && (number <= child.LimitMax))
                            {
                                ret = child[number];
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
                _children = new List<PdfNumberTree>();
                foreach (PdfObjectReference reference in kids.Objects)
                    _children.Add(new PdfNumberTree(Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference), false));

                // Only the root calculates the limits by examining the children
                if (_root)
                {
                    LimitMin = _children[0].LimitMin;
                    LimitMax = _children[_children.Count - 1].LimitMax;
                }
            }
            else
            {
                // Without 'Kids' the 'Nums' is mandatory
                PdfArray array = _dictionary.MandatoryValue<PdfArray>("Nums");

                _nums = new Dictionary<int, PdfObject>();
                int count = array.Objects.Count;
                for (int i = 0; i < count; i += 2)
                {
                    PdfInteger name = (PdfInteger)array.Objects[i];
                    _nums.Add(name.Value, array.Objects[i + 1]);

                    // Only the root calculates the limits by examining the enties
                    if (_root)
                    {
                        if (i == 0)
                            LimitMin = name.Value;
                        else if (i == (count - 1))
                            LimitMax = name.Value;
                    }
                }
            }
        }
    }
}
