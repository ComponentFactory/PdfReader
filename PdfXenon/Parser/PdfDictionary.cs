using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDictionary : PdfObject
    {
        private Dictionary<string, PdfDictEntry> _entries;

        public PdfDictionary(long position, Dictionary<string, PdfDictEntry> entries)
        {
            DictionaryOpenPosition = position;
            _entries = entries;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"PdfDictionary: Count:{Count} (");

            foreach (PdfDictEntry entry in _entries.Values)
                sb.AppendLine($"    {entry.Name.Name} = {entry.Object.ToString()}");

            sb.AppendLine(")");

            return sb.ToString();
        }

        public int Count { get => _entries.Count; }

        public PdfDictEntry this[string key]
        {
            get { return _entries[key]; }
            set { _entries[key] = value; }
        }

        public bool ContainsKey(string key)
        {
            return _entries.ContainsKey(key);
        }

        public bool TryGetValue(string key, out PdfObject obj)
        {
            PdfDictEntry entry;
            if (_entries.TryGetValue(key, out entry))
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

    public class PdfDictEntry
    {
        public PdfName Name { get; set; }
        public PdfObject Object { get; set; }
    }
}
