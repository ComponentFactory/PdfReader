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
            document.Close();

            PdfCatalog catalog = document.Catalog;

            PdfPagesTree pages = catalog.Pages;
            Console.WriteLine(pages.Count);
            for (int i = 0; i < pages.Count; i++)
                Console.WriteLine(pages[i].ToString());

            Console.ReadLine();
        }
    }
}
