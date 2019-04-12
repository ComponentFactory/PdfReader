using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDocument
    {
        private bool _open;
        private Stream _stream;
        private StreamReader _reader;
        private Parser _parser;
        private ParseObjectReference _catalog;
        private ParseObjectReference _info;

        public PdfDocument()
        {
            Version = new PdfVersion() { Major = 1, Minor = 7 };
            IndirectObjects = new PdfIndirectObjects(this);
        }

        public PdfVersion Version { get; private set; }
        public PdfIndirectObjects IndirectObjects { get; private set; }

        public void Load(string filename, bool immediate = false)
        {
            if (_open)
                throw new ApplicationException("Cannot load when document already has a stream open.");

            _reader = new StreamReader(filename);
            Load(_reader.BaseStream, immediate);
        }

        public void Load(Stream stream, bool immediate = false)
        {
            if (_open)
                throw new ApplicationException("Cannot load when document already has a stream open.");

            try
            {
                _stream = stream;
                _parser = new Parser(_stream);
                _parser.ResolveReference += Parser_ResolveReference;

                // PDF file should have a well known marker at top of file
                int versionMajor = 0;
                int versionMinor = 0;
                _parser.ParseHeader(out versionMajor, out versionMinor);
                Version.Major = versionMajor;
                Version.Minor = versionMinor;

                // Find stream position of the last cross-reference table
                long xRefPosition = _parser.ParseXRefOffset();

                bool lastHeader = true;

                do
                {
                    // Get the aggregated set of entries from all the cross-reference table sections
                    List<TokenXRefEntry> xrefs = _parser.ParseXRef(xRefPosition);

                    ParseDictionary trailer = _parser.ParseTrailer();
                    ParseInteger size = trailer.MandatoryValue<ParseInteger>("Size");
                    ParseInteger prev = trailer.OptionalValue<ParseInteger>("Prev");

                    if (lastHeader)
                    {
                        // We only care about the latest defined catalog and information dictionary
                        _catalog = trailer.MandatoryValue<ParseObjectReference>("Root");
                        _info = trailer.OptionalValue<ParseObjectReference>("Info");
                    }

                    foreach (TokenXRefEntry xref in xrefs)
                    {
                        // Ignore unused entries and entries smaller than the defined size from the trailer dictionary
                        if (xref.Used && (xref.Id < size.Integer))
                            IndirectObjects.AddXRef(xref);
                    }

                    // If there is a previous cross-reference table, then we want to process that as well
                    if (prev != null)
                        xRefPosition = prev.Integer;
                    else
                        xRefPosition = 0;

                    lastHeader = false;

                } while (xRefPosition > 0);

                _open = true;

                if (immediate)
                {
                    foreach (var id in IndirectObjects.Values)
                    {
                        foreach (var gen in id.Values)
                            ResolveReference(gen.Id, gen.Gen);
                    }

                    Close();
                }
            }
            catch
            {
                Close();
            }
        }

        public void Close()
        {
            if (_open)
            {
                if (_reader != null)
                {
                    _reader.Dispose();
                    _reader = null;
                }

                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }

                if (_parser != null)
                {
                    _parser.Dispose();
                    _parser = null;
                }

                _open = false;
            }
        }

        private ParseObject ResolveReference(int id, int gen)
        {
            PdfIndirectObjectGen indirect = IndirectObjects[id, gen];
            if (indirect != null)
            {
                if (indirect.Child == null)
                {
                    indirect.Child = _parser.ParseIndirectObject(indirect.Offset).Child;
                    Console.WriteLine(indirect.Child);
                }

                return indirect.Child;
            }

            return null;
        }

        private void Parser_ResolveReference(object sender, ParseResolveEventArgs e)
        {
            e.Object = ResolveReference(e.Id, e.Gen);
        }
    }
}
