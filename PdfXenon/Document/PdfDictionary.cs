using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfDictionary : PdfObject
    {
        private Dictionary<string, PdfObject> _wrapped;

        public PdfDictionary(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
            _wrapped = new Dictionary<string, PdfObject>();
        }

        public ParseDictionary ParseDictionary { get => ParseObject as ParseDictionary; }
        public int Count { get => ParseDictionary.Count; }
        public bool ContainsName(string name) { return ParseDictionary.ContainsName(name); }

        public Dictionary<string, PdfObject>.KeyCollection Names
        {
            get
            {
                WrapAllNames();
                return _wrapped.Keys;
            }
        }

        public Dictionary<string, PdfObject>.ValueCollection Values
        {
            get
            {
                WrapAllNames();
                return _wrapped.Values;
            }
        }

        public Dictionary<string, PdfObject>.Enumerator GetEnumerator()
        {
            WrapAllNames();
            return _wrapped.GetEnumerator();
        }

        public PdfObject this[string name]
        {
            get
            {
                WrapName(name);
                return _wrapped[name];
            }
        }

        public T OptionalValue<T>(string name) where T : PdfObject
        {
            if (ParseDictionary.ContainsName(name))
            {
                WrapName(name);
                if (_wrapped.TryGetValue(name, out PdfObject entry))
                {
                    if (entry is T)
                        return (T)entry;
                    else
                        throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
                }
            }

            return null;
        }

        public PdfDateTime OptionalDateTime(string name)
        {
            PdfString str = OptionalValue<PdfString>(name);
            if (str != null)
                return new PdfDateTime(this, str);

            return null;
        }

        public T MandatoryValue<T>(string name) where T : PdfObject
        {
            if (ParseDictionary.ContainsName(name))
            {
                WrapName(name);
                if (_wrapped.TryGetValue(name, out PdfObject entry))
                {
                    if (entry is T)
                        return (T)entry;
                    else
                        throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
                }
                else
                    throw new ApplicationException($"Dictionary is missing mandatory name '{name}'.");
            }
            else
                throw new ApplicationException($"Dictionary is missing mandatory name '{name}'.");
        }
    
        private void WrapName(string name)
        {
            if (!_wrapped.ContainsKey(name))
                _wrapped.Add(name, WrapObject(ParseDictionary[name]));
        }

        private void WrapAllNames()
        {
            // Are there any dictionary entries that still need wrapping?
            if (ParseDictionary.Count > _wrapped.Count)
            {
                foreach (var name in ParseDictionary.Names)
                {
                    if (!_wrapped.ContainsKey(name))
                        _wrapped.Add(name, WrapObject(ParseDictionary[name]));
                }
            }
        }
    }
}
