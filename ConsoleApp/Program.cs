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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            PdfDocument document = new PdfDocument();
            document.Load(@"d:\PDF17.pdf", true);
            document.Close();
            sw.Stop();

            Console.WriteLine($"Process {sw.ElapsedMilliseconds}ms\n");

            Console.WriteLine($"Pages: {document.Catalog.Pages.Count}");
            Console.WriteLine($"Author: {document.Info.Author}");
            Console.WriteLine($"CreationDate: {document.Info.CreationDate}");
            Console.WriteLine($"Creator: {document.Info.Creator}");
            Console.WriteLine($"Keywords: {document.Info.Keywords}");
            Console.WriteLine($"ModDate: {document.Info.ModDate}");
            Console.WriteLine($"Producer: {document.Info.Producer}");
            Console.WriteLine($"Subject: {document.Info.Subject}");
            Console.WriteLine($"Title: {document.Info.Title}");

            PdfPage page = document.Catalog.Pages[0];
            PdfContents contents = page.Contents;
            PdfContentsParser parser = contents.CreateParser();

            PdfObject obj = null;
            do
            {
                obj = parser.GetToken();
                if (obj != null)
                    Console.WriteLine(obj.ToString());

            } while (obj != null);


            Console.ReadLine();
        }
    }
}
