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
            document.Load(@"d:\Slides.pdf", false);

            PdfCatalog catalog = document.Catalog;
            Console.WriteLine(catalog);

            PdfInfo info = document.Info;
            if (info != null)
            {
                Console.WriteLine("Info");
                Console.WriteLine("----");
                Console.WriteLine(info.Author?.ToString());
                Console.WriteLine(info.Subject?.ToString());
                Console.WriteLine(info.Keywords?.ToString());
                Console.WriteLine(info.Creator?.ToString());
                Console.WriteLine(info.Producer?.ToString());
                Console.WriteLine(info.CreationDate?.ToString());
                Console.WriteLine(info.ModDate?.ToString());
            }

            //PdfPagesTree pages = catalog.Pages;
            //for (int i = 0; i < pages.Count; i++)
            //{
            //    PdfPage page = pages[i];

            //    Console.WriteLine("Page");
            //    Console.WriteLine("----");
            //    Console.WriteLine(page.Resources.ToString());
            //    Console.WriteLine(page.MediaBox.ToString());
            //    Console.WriteLine(page.Contents.ToString());
            //}

            Console.ReadLine();
        }
    }
}
