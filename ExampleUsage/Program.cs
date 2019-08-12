using PdfReader;
using System;
using System.Diagnostics;
using System.IO;

namespace ExampleConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"Example.pdf";
            //-----------------------------------------------
            // Comment/Uncomment the example you want to run
            //-----------------------------------------------

            // 1 - Show document details in compact form
            //---------------------------------------------
            //   LoadImmediately(filename, false, false);

            // 2 - As above but also resolve references
            //---------------------------------------------
            //  LoadImmediately(filename, true, false);

            // 3 - As above but also show stream contents
            //---------------------------------------------
            //  LoadImmediately(filename, true, true);

            // 4 - On demand versions of the above three methods
            //---------------------------------------------
            //  LoadOnDemand(filename, false, false);
            //  LoadOnDemand(filename, true, false);
            //  LoadOnDemand(filename, true, true);

            // 5 - Show content commands for the first page
            //---------------------------------------------
            //  ShowFirstPageContent(filename);

            // 6 - List the indirect objects
            //---------------------------------------------
            //  ListIndirectObjects(filename, false, false);

            Console.ReadLine();
        }

        static private void LoadImmediately(string filename, bool resolve, bool streamContent)
        {
            PdfDocument document = new PdfDocument();
            document.Load(filename, true);
            document.Close();

            // Output an overview of the document contents
            PdfDebugBuilder builder = new PdfDebugBuilder(document);
            builder.Resolve = resolve;
            builder.StreamContent = streamContent;
            Console.WriteLine(builder.ToString());
        }

        static private void LoadOnDemand(string filename, bool resolve, bool streamContent)
        {
            PdfDocument document = new PdfDocument();
            document.Load(filename, false);

            PdfDebugBuilder builder = new PdfDebugBuilder(document);
            builder.Resolve = resolve;
            builder.StreamContent = streamContent;
            Console.WriteLine(builder.ToString());

            // Cannot close document until finished accessing it
            document.Close();
        }

        static private void ShowFirstPageContent(string filename)
        {
            PdfDocument document = new PdfDocument();
            document.Load(filename, false);

            PdfContents contents = document.Catalog.Pages[0].Contents;

            // Get a parser for decoding the contents of the page
            PdfContentsParser parser = contents.CreateParser();

            // Keep getting new content commands until no more left
            PdfObject obj = null;
            while ((obj = parser.GetObject()) != null)
                Console.WriteLine(obj);

            document.Close();
        }

        static private void ListIndirectObjects(string filename, bool resolve, bool streamContent)
        {
            PdfDocument document = new PdfDocument();
            document.Load(filename, true);
            document.Close();

            // Get each indirect object identifier
            foreach(var id in document.IndirectObjects)
            {
                // Get each generation for the identifier
                foreach (var gen in id.Value)
                {
                    PdfDebugBuilder builder = new PdfDebugBuilder(gen.Value);
                    builder.Resolve = resolve;
                    builder.StreamContent = streamContent;
                    Console.WriteLine(builder.ToString());
                }
            }
        }
    }
}
