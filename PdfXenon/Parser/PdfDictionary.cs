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

        public int Count { get => _entries.Count; }

        public PdfDictEntry this[string key]
        {
            get { return _entries[key]; }
            set { _entries[key] = value; }
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
