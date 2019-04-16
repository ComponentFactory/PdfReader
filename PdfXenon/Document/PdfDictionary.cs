using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfDictionary : PdfObject
    {
        private ParseDictionary _dictionary;

        public PdfDictionary(PdfDocument doc, ParseDictionary dictionary)
            : base(doc)
        {
            _dictionary = dictionary;
        }

        public override string ToString()
        {
            return $"PdfDictionary\n{_dictionary.ToString()}";
        }

        public int Count { get => _dictionary.Count; }
        public Dictionary<string, ParseDictEntry>.KeyCollection Keys { get => _dictionary.Keys; }
        public Dictionary<string, ParseDictEntry>.ValueCollection Values { get => _dictionary.Values; }
        public Dictionary<string, ParseDictEntry>.Enumerator GetEnumerator() => _dictionary.GetEnumerator();

        public bool ContainsName(string name)
        {
            return _dictionary.ContainsName(name);
        }

        public ParseDictEntry this[string name]
        {
            get { return _dictionary[name]; }
            set { _dictionary[name] = value; }
        }

        public T OptionalValue<T>(string name) where T : ParseObject
        {
            return _dictionary.OptionalValue<T>(name);
        }

        public T MandatoryValue<T>(string name) where T : ParseObject
        {
            return _dictionary.MandatoryValue<T>(name);
        }
    }
}
