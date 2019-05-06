using PdfXenon.Standard;
using System;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFileLoads(@"d:\Blank.pdf");
            TestFileLoads(@"d:\Coffee.pdf");
            TestFileLoads(@"d:\FSharp.pdf");
            TestFileLoads(@"d:\Magazine.pdf");
            TestFileLoads(@"d:\Maths.pdf");
            TestFileLoads(@"d:\Slides.pdf");
            TestFileLoads(@"d:\PDF17.pdf");

            Console.ReadLine();
        }

        static private void TestFileLoads(string filename)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            PdfDocument document = new PdfDocument();
            document.Load(filename, true);
            document.Close();

            foreach (PdfPage page in document.Catalog.Pages)
            {
                PdfContents contents = page.Contents;
                PdfContentsParser parser = contents.CreateParser();

                PdfObject obj = null;
                do
                {
                    obj = parser.GetObject();
                } while (obj != null);
            }

            sw.Stop();

            Console.WriteLine("");
            Console.WriteLine($"{filename}: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine(document.Catalog.Pages[document.Catalog.Pages.Count / 2].ToDebug());
        }
    }
}
