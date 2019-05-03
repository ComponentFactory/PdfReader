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
                    obj = parser.GetToken();
                } while (obj != null);
            }

            sw.Stop();

            Console.WriteLine("");
            Console.WriteLine($"{filename}: {sw.ElapsedMilliseconds}ms");
            Output(document.Catalog.Pages[0]);
        }

        static private void Output(PdfDocument document)
        {
            Console.WriteLine($"Version: {document.Catalog.Version}");
            Console.WriteLine($"Pages: {document.Catalog.Pages.Count}");
            Console.WriteLine($"PageLabels: {document.Catalog.PageLabels}");
            Console.WriteLine($"Names: {document.Catalog.Names}");
            Console.WriteLine($"Dests: {document.Catalog.Dests}");
            Console.WriteLine($"ViewerPreferences: {document.Catalog.ViewerPreferences}");
            Console.WriteLine($"PageLayout: {document.Catalog.PageLayout}");
            Console.WriteLine($"PageMode: {document.Catalog.PageMode}");
            Console.WriteLine($"Outlines: {document.Catalog.Outlines}");
            Console.WriteLine($"Threads: {document.Catalog.Threads}");
            Console.WriteLine($"OpenAction: {document.Catalog.OpenAction}");
            Console.WriteLine($"AA: {document.Catalog.AA}");
            Console.WriteLine($"URI: {document.Catalog.URI}");
            Console.WriteLine($"AcroForm: {document.Catalog.AcroForm}");
            Console.WriteLine($"Metadata: {document.Catalog.Metadata}");
            Console.WriteLine($"MarkInfo: {document.Catalog.MarkInfo}");
            Console.WriteLine($"Lang: {document.Catalog.Lang}");
            Console.WriteLine($"SpiderInfo: {document.Catalog.SpiderInfo}");
            Console.WriteLine($"OutputIntents: {document.Catalog.OutputIntents}");
            Console.WriteLine($"PieceInfo: {document.Catalog.PieceInfo}");
            Console.WriteLine($"OCProperties: {document.Catalog.OCProperties}");
            Console.WriteLine($"Perms: {document.Catalog.Perms}");
            Console.WriteLine($"Legal: {document.Catalog.Legal}");
            Console.WriteLine($"Requirements: {document.Catalog.Requirements}");
            Console.WriteLine($"Collection: {document.Catalog.Collection}");
            Console.WriteLine($"NeedsRendering: {document.Catalog.NeedsRendering}");
        }

        static private void Output(PdfPage page)
        {
            Console.WriteLine($"LastModified: {page.LastModified}");
            Console.WriteLine($"Resources: {page.Resources}");
            Console.WriteLine($"MediaBox: {page.MediaBox}");
            Console.WriteLine($"CropBox: {page.CropBox}");
            Console.WriteLine($"BleedBox: {page.BleedBox}");
            Console.WriteLine($"TrimBox: {page.TrimBox}");
            Console.WriteLine($"ArtBox: {page.ArtBox}");
            Console.WriteLine($"BoxColorInfo: {page.BoxColorInfo}");
            Console.WriteLine($"Contents: {page.Contents}");
            Console.WriteLine($"Rotate: {page.Rotate}");
            Console.WriteLine($"Group: {page.Group}");
            Console.WriteLine($"Thumb: {page.Thumb}");
            Console.WriteLine($"B: {page.B}");
            Console.WriteLine($"Dur: {page.Dur}");
            Console.WriteLine($"Trans: {page.Trans}");
            Console.WriteLine($"Annots: {page.Annots}");
            Console.WriteLine($"AA: {page.AA}");
            Console.WriteLine($"Metadata: {page.Metadata}");
            Console.WriteLine($"PieceInfo: {page.PieceInfo}");
            Console.WriteLine($"StructParents: {page.StructParents}");
            Console.WriteLine($"ID: {page.ID}");
            Console.WriteLine($"PZ: {page.PZ}");
            Console.WriteLine($"SeparationInfo: {page.SeparationInfo}");
            Console.WriteLine($"Tabs: {page.Tabs}");
            Console.WriteLine($"TemplateInstantiated: {page.TemplateInstantiated}");
            Console.WriteLine($"PresSteps: {page.PresSteps}");
            Console.WriteLine($"UserUnit: {page.UserUnit}");
            Console.WriteLine($"VP: {page.VP}");
        }
    }
}
