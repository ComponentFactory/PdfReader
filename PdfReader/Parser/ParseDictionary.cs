using System;
using System.Collections.Generic;
using System.Text;

namespace PdfReader
{
    public class ParseDictionary : ParseObjectBase
    {
        private List<string> _names;
        private List<ParseObjectBase> _values;
        private Dictionary<string, ParseObjectBase> _dictionary;

        public ParseDictionary(List<string> names, List<ParseObjectBase> values)
        {
            _names = names;
            _values = values;
        }

        public int Count { get => _names != null ? _names.Count : _dictionary.Count; }

        public bool ContainsName(string name)
        {
            BuildDictionary();
            return _dictionary.ContainsKey(name);
        }

        public Dictionary<string, ParseObjectBase>.KeyCollection Keys
        {
            get
            {
                BuildDictionary();
                return _dictionary.Keys;
            }
        }

        public Dictionary<string, ParseObjectBase>.ValueCollection Values
        {
            get
            {
                BuildDictionary();
                return _dictionary.Values;
            }
        }

        public Dictionary<string, ParseObjectBase>.Enumerator GetEnumerator()
        {
            BuildDictionary();
            return _dictionary.GetEnumerator();
        }

        public ParseObjectBase this[string name]
        {
            get
            {
                BuildDictionary();
                return _dictionary[name];
            }

            set
            {
                BuildDictionary();
                _dictionary[name] = value;
            }
        }

        public T OptionalValue<T>(string name) where T : ParseObjectBase
        {
            BuildDictionary();
            if (_dictionary.TryGetValue(name, out ParseObjectBase entry))
            {
                if (entry is T)
                    return (T)entry;
                else
                    throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
            }

            return null;
        }

        public T MandatoryValue<T>(string name) where T : ParseObjectBase
        {
            BuildDictionary();
            if (_dictionary.TryGetValue(name, out ParseObjectBase entry))
            {
                if (entry is T)
                    return (T)entry;
                else
                    throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
            }
            else
                throw new ApplicationException($"Dictionary is missing mandatory name '{name}'.");
        }

        private void BuildDictionary()
        {
            if (_dictionary == null)
            {
                _dictionary = new Dictionary<string, ParseObjectBase>();

                int count = Count;
                for(int i=0; i<count; i++)
                    _dictionary.Add(_names[i], _values[i]);

                _names = null;
                _values = null;
            }
        }
    }
}
