using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseDictionary : ParseObject
    {
        private Dictionary<string, ParseDictEntry> _entries;

        public ParseDictionary(long position, Dictionary<string, ParseDictEntry> entries)
        {
            DictionaryOpenPosition = position;
            _entries = entries;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"ParseDictionary ({Position}): Count:{Count} (");

            foreach (ParseDictEntry entry in _entries.Values)
                sb.AppendLine($"    {entry.Name.Name} = {entry.Object.ToString()}");

            sb.AppendLine(")");

            return sb.ToString();
        }

        public int Count { get => _entries.Count; }

        public bool ContainsName(string name)
        {
            return _entries.ContainsKey(name);
        }

        public ParseDictEntry this[string name]
        {
            get { return _entries[name]; }
            set { _entries[name] = value; }
        }

        public bool TryGetValue(string name, out ParseObject obj)
        {
            ParseDictEntry entry;
            if (_entries.TryGetValue(name, out entry))
            {
                obj = entry.Object;
                return true;
            }
            else
            {
                obj = null;
                return false;
            }
        }

        public override long Position { get => DictionaryOpenPosition; }

        private long DictionaryOpenPosition { get; set; }
    }

    public class ParseDictEntry
    {
        public ParseName Name { get; set; }
        public ParseObject Object { get; set; }
    }
}
