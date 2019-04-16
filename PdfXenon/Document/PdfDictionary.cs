using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfDictionary : PdfObject
    {
        public PdfDictionary(PdfDocument doc, ParseDictionary parse)
            : base(doc)
        {
            ParseDictionary = parse;
        }

        public override string ToString()
        {
            return $"PdfDictionary\n{ParseDictionary.ToString()}";
        }

        public ParseDictionary ParseDictionary { get; private set; }

        public int Count { get => ParseDictionary.Count; }
        public bool ContainsName(string name) { return ParseDictionary.ContainsName(name); }
        public Dictionary<string, ParseDictEntry>.KeyCollection Names { get => ParseDictionary.Names; }
        public Dictionary<string, ParseDictEntry>.ValueCollection Values { get => ParseDictionary.Values; }
        public Dictionary<string, ParseDictEntry>.Enumerator GetEnumerator() => ParseDictionary.GetEnumerator();
        public ParseDictEntry this[string name] { get => ParseDictionary[name]; }

        public T OptionalValue<T>(string name) where T : ParseObject
        {
            return ParseDictionary.OptionalValue<T>(name);
        }

        public PdfDateTime OptionalDateTime(string name)
        {
            ParseString parse = OptionalValue<ParseString>(name);
            if (parse != null)
            {
                try
                {
                    // Some files seem to have an invalid format for the date
                    return new PdfDateTime(Doc, parse);
                }
                catch { }
            }

            return null;
        }

        public T MandatoryValue<T>(string name) where T : ParseObject
        {
            return ParseDictionary.MandatoryValue<T>(name);
        }
    }
}
