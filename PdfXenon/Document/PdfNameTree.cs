using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfNameTree : PdfObject
    {
        private Dictionary<string, PdfObject> _values = new Dictionary<string, PdfObject>();

        public PdfNameTree(PdfObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            Names = new List<string>();
            ProcessNameTreePage(dictionary);
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

        public int Count { get => Names.Count; }
        public bool ContainsName(string name) { return _values.ContainsKey(name); }
        public List<string> Names { get; private set; }
        public Dictionary<string, PdfObject>.ValueCollection Values { get { return _values.Values; } }
        public Dictionary<string, PdfObject>.Enumerator GetEnumerator() { return _values.GetEnumerator(); }
        public PdfObject this[string name] { get => _values[name]; }

        private void ProcessNameTreePage(PdfDictionary dictionary)
        {
            PdfArray kids = dictionary.OptionalValue<PdfArray>("Kids");
            if (kids != null)
            {
                // Process each child number tree page
                foreach (PdfObjectReference reference in kids.Objects)
                    ProcessNameTreePage(Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference));
            }
            else
            {
                // Without a 'Kids' there must be 'Names'
                PdfArray names = dictionary.MandatoryValue<PdfArray>("Names");
                for(int i=0; i<names.Objects.Count; i+=2)
                {
                    PdfString name = (PdfString)names.Objects[i];
                    Names.Add(name.Value);
                    _values.Add(name.Value, names.Objects[i + 1]);
                }
            }
        }
    }
}
