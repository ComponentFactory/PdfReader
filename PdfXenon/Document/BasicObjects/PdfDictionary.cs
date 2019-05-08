using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDictionary : PdfObject
    {
        private Dictionary<string, PdfObject> _wrapped;

        public PdfDictionary(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public override string ToString()
        {
            return $"PdfDictionary Count:{ParseDictionary.Count}";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            WrapAllNames();
            string blank = new string(' ', indent);

            sb.Append("<<");
            indent += 2;

            int index = 0;
            int count = _wrapped.Count;
            foreach (KeyValuePair<string, PdfObject> entry in _wrapped)
            {
                if ((index == 1) && (count == 2))
                    sb.Append(" ");
                else if (index > 0)
                    sb.Append("  ");

                sb.Append($"{entry.Key} ");
                int entryIndent = entry.Key.Length + 1;
                entry.Value.ToDebug(sb, entryIndent);

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

        public ParseDictionary ParseDictionary { get => ParseObject as ParseDictionary; }
        public int Count { get => ParseDictionary.Count; }
        public bool ContainsName(string name) { return ParseDictionary.ContainsName(name); }

        public Dictionary<string, PdfObject>.KeyCollection Keys
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

        public T OptionalValueRef<T>(string name) where T : PdfObject
        {
            if (ParseDictionary.ContainsName(name))
            {
                WrapName(name);
                if (_wrapped.TryGetValue(name, out PdfObject entry))
                {
                    if (entry is PdfObjectReference reference)
                    {
                        if (Document.IndirectObjects.ContainsId(reference.Id))
                        {
                            PdfIndirectObjectId id = Document.IndirectObjects[reference.Id];
                            if (id.ContainsGen(reference.Gen))
                            {
                                entry = Document.ResolveReference(reference);
                                if (entry is T)
                                    return (T)entry;
                                else
                                    throw new ApplicationException($"Dictionary entry is type '{entry.GetType().Name}' instead of mandatory type of '{typeof(T).Name}'.");
                            }
                        }
                    }
                    else  if (entry is T)
                        return (T)entry;

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

        public T MandatoryValueRef<T>(string name) where T : PdfObject
        {
            if (ParseDictionary.ContainsName(name))
            {
                WrapName(name);
                if (_wrapped.TryGetValue(name, out PdfObject entry))
                {
                    if (entry is PdfObjectReference reference)
                    {
                        entry = Document.ResolveReference(reference);
                        if (entry is T)
                            return (T)entry;
                    }
                    else if (entry is T)
                        return (T)entry;

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
            if (_wrapped == null)
                _wrapped = new Dictionary<string, PdfObject>();

            if (!_wrapped.ContainsKey(name))
                _wrapped.Add(name, WrapObject(ParseDictionary[name]));
        }

        private void WrapAllNames()
        {
            if (_wrapped == null)
                _wrapped = new Dictionary<string, PdfObject>();

            // Are there any dictionary entries that still need wrapping?
            if (ParseDictionary.Count > _wrapped.Count)
            {
                foreach (var name in ParseDictionary.Keys)
                {
                    if (!_wrapped.ContainsKey(name))
                        _wrapped.Add(name, WrapObject(ParseDictionary[name]));
                }
            }
        }
    }
}
