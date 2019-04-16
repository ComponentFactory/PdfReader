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

            foreach(PdfPage page in document.Catalog.Pages)
            {
                Console.WriteLine("Page");
                Console.WriteLine("----");
                Console.WriteLine(page.Resources.ToString());
                Console.WriteLine(page.MediaBox.ToString());
                Console.WriteLine(page.Contents.ToString());
            }

            Console.ReadLine();
        }
    }
}
