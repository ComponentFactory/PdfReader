using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNumberTree : PdfObject
    {
        private Dictionary<int, PdfObject> _values = new Dictionary<int, PdfObject>();

        public PdfNumberTree(PdfObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            Numbers = new List<int>();
            ProcessNumberTreePage(dictionary);
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);
            foreach(var pair in _values)
            {
                string pre = $"{pair.Key} = ";
                sb.Append(pre);
                pair.Value.Output(sb, indent + pre.Length);
                sb.Append("\n");
                sb.Append(blank);
            }

            return indent;
        }

        public int Count { get => Numbers.Count; }
        public bool ContainsNumber(int number) { return _values.ContainsKey(number); }
        public List<int> Numbers { get; private set; }
        public Dictionary<int, PdfObject>.ValueCollection Values { get { return _values.Values; } }
        public Dictionary<int, PdfObject>.Enumerator GetEnumerator() { return _values.GetEnumerator(); }
        public PdfObject this[int number] { get => _values[number]; }

        private void ProcessNumberTreePage(PdfDictionary dictionary)
        {
            PdfArray kids = dictionary.OptionalValue<PdfArray>("Kids");
            if (kids != null)
            {
                // Process each child number tree page
                foreach (PdfObjectReference reference in kids.Objects)
                    ProcessNumberTreePage(Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference));
            }
            else
            {
                // Without a 'Kids' there must be 'Nums'
                PdfArray nums = dictionary.MandatoryValue<PdfArray>("Nums");
                for(int i=0; i<nums.Objects.Count; i+=2)
                {
                    PdfInteger key = (PdfInteger)nums.Objects[i];
                    Numbers.Add(key.Value);
                    _values.Add(key.Value, nums.Objects[i + 1]);
                }
            }
        }
    }
}
