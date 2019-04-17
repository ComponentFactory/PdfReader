using PdfXenon.Standard;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            PdfDocument document = new PdfDocument();
            document.Load(@"d:\PDF17.pdf", false);

            Console.WriteLine($"Pages: {document.Catalog.Pages.Count}");
            Console.WriteLine($"Author: {document.Info.Author}");
            Console.WriteLine($"CreationDate: {document.Info.CreationDate}");
            Console.WriteLine($"Creator: {document.Info.Creator}");
            Console.WriteLine($"Keywords: {document.Info.Keywords}");
            Console.WriteLine($"ModDate: {document.Info.ModDate}");
            Console.WriteLine($"Producer: {document.Info.Producer}");
            Console.WriteLine($"Subject: {document.Info.Subject}");
            Console.WriteLine($"Title: {document.Info.Title}");

            Console.ReadLine();
        }
    }
}
