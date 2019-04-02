using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDictionary : PdfObject
    {
        public PdfDictionary(long position)
        {
            DictionaryOpenPosition = position;
        }

        public override long Position { get => DictionaryOpenPosition; }

        private long DictionaryOpenPosition { get; set; }
    }
}
