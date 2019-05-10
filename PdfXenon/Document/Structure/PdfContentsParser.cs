using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfContentsParser : PdfObject
    {
        private int _index = 0;
        private Parser _parser;

        public PdfContentsParser(PdfContents parent)
            : base(parent)
        {
        }

        public PdfContents Contents { get => TypedParent<PdfContents>(); }

        public PdfObject GetObject()
        {
            // First time around we setup the parser to the first stream
            if ((_parser == null) && (_index < Contents.Streams.Count))
                _parser = new Parser(new MemoryStream(Contents.Streams[_index++].ValueAsBytes), true);

            // Keep trying to get a parsed object as long as there is a parser for a stream
            while (_parser != null)
            {
                ParseObject obj = _parser.ParseObject(true);
                if (obj != null)
                    return WrapObject(obj);

                _parser.Dispose();
                _parser = null;

                // Is there another stream we can continue parsing with
                if (_index < Contents.Streams.Count)
                    _parser = new Parser(new MemoryStream(Contents.Streams[_index++].ValueAsBytes), true);
            }

            return null;
        }
    }
}
