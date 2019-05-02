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
            TestFileLoads(@"d:\PDF17.pdf");
            TestFileLoads(@"d:\Slides.pdf");

            //Console.WriteLine($"Pages: {document.Catalog.Pages.Count}");
            //Console.WriteLine($"Author: {document.Info.Author}");
            //Console.WriteLine($"CreationDate: {document.Info.CreationDate}");
            //Console.WriteLine($"Creator: {document.Info.Creator}");
            //Console.WriteLine($"Keywords: {document.Info.Keywords}");
            //Console.WriteLine($"ModDate: {document.Info.ModDate}");
            //Console.WriteLine($"Producer: {document.Info.Producer}");
            //Console.WriteLine($"Subject: {document.Info.Subject}");
            //Console.WriteLine($"Title: {document.Info.Title}");

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
                    obj = parser.GetToken();
                } while (obj != null);
            }

            sw.Stop();
            Console.WriteLine($"{filename}: {sw.ElapsedMilliseconds}ms");
        }
    }
}
