using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseDictionary : ParseObject
    {
        private Dictionary<string, ParseDictEntry> _entries;

        public ParseDictionary(long position, Dictionary<string, ParseDictEntry> entries)
            : base(position)
        {
            _entries = entries;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"ParseDictionary ({Position}): Count:{Count} (");

            foreach (ParseDictEntry entry in _entries.Values)
                sb.AppendLine($"    {entry.Name.Value} = {entry.Object.ToString()}");

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

        public T OptionalValue<T>(string name) where T : ParseObject
        {
            ParseDictEntry entry;
            if (_entries.TryGetValue(name, out entry))
            {
                if (entry.Object is T)
                    return entry.Object as T;
                else
                    throw new ApplicationException($"Dictionary entry is type '{entry.Object.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
            }

            return null;
        }

        public T MandatoryValue<T>(string name) where T : ParseObject
        {
            ParseDictEntry entry;
            if (_entries.TryGetValue(name, out entry))
            {
                if (entry.Object is T)
                    return entry.Object as T;
                else
                    throw new ApplicationException($"Dictionary entry is type '{entry.Object.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
            }
            else
                throw new ApplicationException($"Dictionary is missing mandatory name '{name}'.");
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
    }

    public class ParseDictEntry
    {
        public ParseName Name { get; set; }
        public ParseObject Object { get; set; }
    }
}
