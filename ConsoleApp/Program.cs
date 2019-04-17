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

            Console.WriteLine(document.Catalog.Pages.Count);
            foreach(PdfPage page in document.Catalog.Pages)
                Console.WriteLine(page.MediaBox.ToString());

            Console.ReadLine();
        }
    }
}
