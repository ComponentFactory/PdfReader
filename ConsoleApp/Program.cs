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
            //TestFileLoads(@"d:\Blank.pdf");
            //TestFileLoads(@"d:\Coffee.pdf");
            //TestFileLoads(@"d:\FSharp.pdf");
            //TestFileLoads(@"d:\Magazine.pdf");
            //TestFileLoads(@"d:\Maths.pdf");
            //TestFileLoads(@"d:\Slides.pdf");
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
                    obj = parser.GetToken();
                } while (obj != null);
            }

            sw.Stop();

            Console.WriteLine("");
            Console.WriteLine($"{filename}: {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Catalog: {document.Catalog}");

            //Console.WriteLine($"Type: {document.Catalog.Type}");
            //Console.WriteLine($"Version: {document.Catalog.Version}");
            //Console.WriteLine($"Pages: {document.Catalog.Pages.Count}");
            //Console.WriteLine($"PageLabels: {document.Catalog.PageLabels}");
            //Console.WriteLine($"Names: {document.Catalog.Names}");
            //Console.WriteLine($"Dests: {document.Catalog.Dests}");
            //Console.WriteLine($"ViewerPreferences: {document.Catalog.ViewerPreferences}");
            //Console.WriteLine($"PageLayout: {document.Catalog.PageLayout}");
            //Console.WriteLine($"PageMode: {document.Catalog.PageMode}");
            //Console.WriteLine($"Outlines: {document.Catalog.Outlines}");
            //Console.WriteLine($"Threads: {document.Catalog.Threads}");
            //Console.WriteLine($"OpenAction: {document.Catalog.OpenAction}");
            //Console.WriteLine($"AA: {document.Catalog.AA}");
            //Console.WriteLine($"URI: {document.Catalog.URI}");
            //Console.WriteLine($"AcroForm: {document.Catalog.AcroForm}");
            //Console.WriteLine($"Metadata: {document.Catalog.Metadata}");
            //Console.WriteLine($"IdTree: {document.Catalog.StructTreeRoot.IDTree}");
            //Console.WriteLine($"ParentTree: {document.Catalog.StructTreeRoot.ParentTree}");
            //Console.WriteLine($"ParentTreeNextKey: {document.Catalog.StructTreeRoot.ParentTreeNextKey}");
            //Console.WriteLine($"RoleMap: {document.Catalog.StructTreeRoot.RoleMap}");
            //Console.WriteLine($"ClassMap: {document.Catalog.StructTreeRoot.ClassMap}");
        }
    }
}
