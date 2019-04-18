using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseDictionary : ParseObject
    {
        private Dictionary<string, ParseObject> _dictionary;

        public ParseDictionary(long position, Dictionary<string, ParseObject> dictionary)
            : base(position)
        {
            _dictionary = dictionary;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);

            sb.Append("<<");
            indent += 2;

            int index = 0;
            int count = _dictionary.Count;
            foreach (KeyValuePair<string, ParseObject> entry in _dictionary)
            {
                if ((index == 1) && (count == 2))
                    sb.Append(" ");
                else if (index > 0)
                    sb.Append("  ");

                sb.Append($"{entry.Key} ");
                int entryIndent = entry.Key.Length + 1;
                entry.Value.Output(sb, entryIndent);

                if (count > 2)
                {
                    sb.Append("\n");
                    sb.Append(blank);
                }

                index++;
            }

            sb.Append(">>");
            return indent;
        }

        public int Count { get => _dictionary.Count; }
        public bool ContainsName(string name) { return _dictionary.ContainsKey(name); }
        public Dictionary<string, ParseObject>.KeyCollection Names { get => _dictionary.Keys; }
        public Dictionary<string, ParseObject>.ValueCollection Values { get => _dictionary.Values; }
        public Dictionary<string, ParseObject>.Enumerator GetEnumerator() => _dictionary.GetEnumerator();

        public ParseObject this[string name]
        {
            get { return _dictionary[name]; }
            set { _dictionary[name] = value; }
        }

        public T OptionalValue<T>(string name) where T : ParseObject
        {
            ParseObject entry;
            if (_dictionary.TryGetValue(name, out entry))
            {
                if (entry is T)
                    return (T)entry;
                else
                    throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
            }

            return null;
        }

        public T MandatoryValue<T>(string name) where T : ParseObject
        {
            ParseObject entry;
            if (_dictionary.TryGetValue(name, out entry))
            {
                if (entry is T)
                    return (T)entry;
                else
                    throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
            }
            else
                throw new ApplicationException($"Dictionary is missing mandatory name '{name}'.");
        }
    }
}
