using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Document
    {
        private Parser Parser { get; set; }

        public Document()
        {
        }

        public int VersionMajor { get; private set; }
        public int VersionMinor { get; private set; }

        public void Load(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
                Load(reader.BaseStream);
        }

        public void Load(Stream stream)
        {
            Parser parser = new Parser(stream);
            parser.ResolveReference += Parser_ResolveReference;

            // PDF file should have a well known marker at top of file
            int versionMajor = 0;
            int versionMinor = 0;
            parser.ParseHeader(out versionMajor, out versionMinor);
            VersionMajor = versionMajor;
            VersionMinor = versionMinor;

            // Find stream position of the last cross-reference table
            long xRefPosition = parser.ParseXRefOffset();

            do
            {
                // Get the aggregated set of entries from all cross-reference table sections
                List<TokenXRefEntry> xrefs = parser.ParseXRef(xRefPosition);

                // If there is a previous cross-reference table, then we want to process that as well
                PdfDictionary trailer = parser.ParseTrailer();
                if (trailer.TryGetValue("Prev", out PdfObject obj))
                {
                    PdfNumeric numeric = obj as PdfNumeric;
                    if ((numeric == null) || !numeric.IsInteger)
                        throw new ApplicationException($"Trailer 'Prev' entry must be an integer value at position {numeric.Position}.");

                    xRefPosition = numeric.Integer;
                }
                else
                    xRefPosition = 0;

                // Load each of the indirect objects
                foreach (TokenXRefEntry xref in xrefs)
                {
                    if (xref.Used)
                    {
                        PdfIndirectObject indirect = parser.ParseIndirectObject(xref.Offset);
                        Console.WriteLine(indirect);
                    }
                }

            } while (xRefPosition > 0);
        }

        private void Parser_ResolveReference(object sender, ResolveEventArgs e)
        {
            // todo
        }
    }
}
