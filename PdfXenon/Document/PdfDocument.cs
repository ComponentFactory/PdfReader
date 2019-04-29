using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDocument : PdfObject
    {
        private bool _open;
        private Stream _stream;
        private StreamReader _reader;
        private Parser _parser;
        private PdfObjectReference _refCatalog;
        private PdfObjectReference _refInfo;
        private PdfCatalog _pdfCatalog;
        private PdfInfo _pdfInfo;

        public PdfDocument()
            : base(null)
        {
            Version = new PdfVersion(this, 0, 0);
            IndirectObjects = new PdfIndirectObjects(this);
            DecryptHandler = new PdfDecryptNone(this);
        }

        public PdfVersion Version { get; private set; }
        public PdfIndirectObjects IndirectObjects { get; private set; }
        public PdfDecrypt DecryptHandler { get; private set; }

        public void Load(string filename, bool immediate = false)
        {
            if (_open)
                throw new ApplicationException("Document already has a stream open.");

            if (immediate)
            {
                // Faster to read all of the file contents at once and then parse, rather than read progressively during parsing
                Load(new MemoryStream(File.ReadAllBytes(filename)), immediate);
            }
            else
            {
                _reader = new StreamReader(filename);
                Load(_reader.BaseStream, immediate);
            }
        }

        public void Load(Stream stream, bool immediate = false)
        {
            if (_open)
                throw new ApplicationException("Document already has a stream open.");

            _stream = stream;
            _parser = new Parser(_stream);
            _parser.ResolveReference += Parser_ResolveReference;

            // PDF file should have a well known marker at top of file
            _parser.ParseHeader(out int versionMajor, out int versionMinor);
            Version = new PdfVersion(this, versionMajor, versionMinor);

            // Find stream position of the last cross-reference table
            long xRefPosition = _parser.ParseXRefOffset();
            bool lastHeader = true;

            do
            {
                // Get the aggregated set of entries from all the cross-reference table sections
                List<TokenXRefEntry> xrefs = _parser.ParseXRef(xRefPosition);

                // Should always be positioned at the trailer after parsing cross-table references
                PdfDictionary trailer = new PdfDictionary(this, _parser.ParseTrailer());
                PdfInteger size = trailer.MandatoryValue<PdfInteger>("Size");
                foreach (TokenXRefEntry xref in xrefs)
                {
                    // Ignore unused entries and entries smaller than the defined size from the trailer dictionary
                    if (xref.Used && (xref.Id < size.Value))
                        IndirectObjects.AddXRef(xref);
                }

                if (lastHeader)
                {
                    // Replace the default decryption handler with one from the document settings
                    DecryptHandler = PdfDecrypt.CreateDecrypt(this, trailer);

                    // We only care about the latest defined catalog and information dictionary
                    _refCatalog = trailer.MandatoryValue<PdfObjectReference>("Root");
                    _refInfo = trailer.OptionalValue<PdfObjectReference>("Info");
                }

                // If there is a previous cross-reference table, then we want to process that as well
                PdfInteger prev = trailer.OptionalValue<PdfInteger>("Prev");
                if (prev != null)
                    xRefPosition = prev.Value;
                else
                    xRefPosition = 0;

                lastHeader = false;

            } while (xRefPosition > 0);

            _open = true;

            if (immediate)
            {
                // Must load all objects immediately so the stream can then be closed
                IndirectObjects.ResolveAllReferences(this);
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

        public PdfCatalog Catalog
        {
            get
            {
                if ((_pdfCatalog == null) && (_refCatalog != null))
                {
                    PdfDictionary dictionary = IndirectObjects.MandatoryValue<PdfDictionary>(_refCatalog);
                    _pdfCatalog = new PdfCatalog(dictionary.Parent, dictionary.ParseObject as ParseDictionary);
                }

                return _pdfCatalog;
            }
        }

        public PdfInfo Info
        {
            get
            {
                if ((_pdfInfo == null) && (_refInfo != null))
                {
                    PdfDictionary dictionary = IndirectObjects.MandatoryValue<PdfDictionary>(_refInfo);
                    _pdfInfo = new PdfInfo(dictionary.Parent, dictionary.ParseObject as ParseDictionary);
                }

                return _pdfInfo;
            }
        }

        public PdfObject ResolveReference(PdfObjectReference reference)
        {
            return ResolveReference(reference.Id, reference.Gen);
        }

        public PdfObject ResolveReference(int id, int gen)
        {
            return ResolveReference(IndirectObjects[id, gen]);
        }

        public PdfObject ResolveReference(PdfIndirectObject indirect)
        {
            if (indirect != null)
            {
                if (indirect.Child == null)
                {
                    ParseIndirectObject parseIndirectObject = _parser.ParseIndirectObject(indirect.Offset);
                    indirect.Child = indirect.WrapObject(parseIndirectObject.Object);
                }

                return indirect.Child;
            }

            return null;
        }

        private void Parser_ResolveReference(object sender, ParseResolveEventArgs e)
        {
            e.Object = ResolveReference(e.Id, e.Gen).ParseObject;
        }
    }
}
