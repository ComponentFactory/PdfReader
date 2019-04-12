using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Document
    {
        private PdfParser Parser { get; set; }
        private Dictionary<int, IndirectObjectId> IndirectObjects { get; set; }

        public Document()
        {
            Parser = null;
            IndirectObjects = new Dictionary<int, IndirectObjectId>();
        }

        public int VersionMajor { get; private set; }
        public int VersionMinor { get; private set; }

        public void Close()
        {
            // todo
        }

        public void Load(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            Load(reader.BaseStream);
        }

        public void Load(Stream stream)
        {
            Parser = new PdfParser(stream);
            Parser.ResolveReference += Parser_ResolveReference;

            // PDF file should have a well known marker at top of file
            int versionMajor = 0;
            int versionMinor = 0;
            Parser.ParseHeader(out versionMajor, out versionMinor);
            VersionMajor = versionMajor;
            VersionMinor = versionMinor;

            // Find stream position of the last cross-reference table
            long xRefPosition = Parser.ParseXRefOffset();

            do
            {
                // Get the aggregated set of entries from all cross-reference table sections
                foreach (TokenXRefEntry xref in Parser.ParseXRef(xRefPosition))
                {
                    // We just ignore free entries as we never reuse them anyway
                    if (xref.Used)
                    {
                        // Add all generations to the same object id entry
                        if (IndirectObjects.TryGetValue(xref.Id, out IndirectObjectId indirect))
                            indirect.Add(xref);
                        else
                            IndirectObjects.Add(xref.Id, new IndirectObjectId(xref));
                    }
                }

                // If there is a previous cross-reference table, then we want to process that as well
                PdfDictionary trailer = Parser.ParseTrailer();
                if (trailer.TryGetValue("Prev", out PdfObject obj))
                {
                    PdfNumeric numeric = obj as PdfNumeric;
                    if ((numeric == null) || !numeric.IsInteger)
                        throw new ApplicationException($"Trailer 'Prev' entry must be an integer value at position {numeric.Position}.");

                    xRefPosition = numeric.Integer;
                }
                else
                    xRefPosition = 0;

            } while (xRefPosition > 0);
        }

        private void Parser_ResolveReference(object sender, PdfResolveEventArgs e)
        {
            if (IndirectObjects.TryGetValue(e.Id, out IndirectObjectId indirect))
            {
                IndirectObjectGen gen = indirect.FindGeneration(e.Gen);
                if (gen != null)
                {
                    if (gen.IndirectObject == null)
                    {
                        gen.IndirectObject = Parser.ParseIndirectObject(gen.Offset);
                        Console.WriteLine(gen.IndirectObject);
                    }

                    e.Object = gen.IndirectObject.Obj;
                }
            }
        }
    }

    public class IndirectObjectId
    {
        private IndirectObjectGen _single;
        private Dictionary<int, IndirectObjectGen> _all;

        public IndirectObjectId(TokenXRefEntry xref)
        {
            _single = new IndirectObjectGen(xref);
        }

        public void Add(TokenXRefEntry xref)
        {
            if (_all == null)
            {
                // Convert from single entry to using a dictionary lookup of multiple entries
                _all = new Dictionary<int, IndirectObjectGen>();
                _all.Add(_single.Gen, _single);
                _single = null;
            }

            _all.Add(xref.Gen, new IndirectObjectGen(xref));
        }

        public IndirectObjectGen FindGeneration(int gen)
        {
            IndirectObjectGen ret = null;

            if (_single != null)
            {
                if (_single.Gen == gen)
                    ret = _single;
            }
            else
                _all.TryGetValue(gen, out ret);

            return ret;
        }
    }

    public class IndirectObjectGen
    {
        private TokenXRefEntry _xRef;

        public IndirectObjectGen(TokenXRefEntry xref)
        {
            _xRef = xref;
        }

        public int Id { get => _xRef.Id; }
        public int Gen { get => _xRef.Gen; }
        public int Offset { get => _xRef.Offset; }
        public PdfIndirectObject IndirectObject { get; set; }
    }
}
