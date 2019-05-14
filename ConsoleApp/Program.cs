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
            //TestFileLoads(@"d:\Shapes.pdf");
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
            PdfDocument document = new PdfDocument();
            document.Load(filename, true);
            document.Close();

            PdfPage page = document.Catalog.Pages[1144];
            Console.WriteLine(new PdfDebugBuilder(page) { Document = document, Resolve = true, StreamContent = true });
            RenderPageResolver processor = new RenderPageResolver(page, new RendererNull());
            processor.Process();
        }
    }
}
