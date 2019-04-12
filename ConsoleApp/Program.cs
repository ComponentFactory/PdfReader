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
            document.Load(@"d:\Blank.pdf", true);
            document.Close();

            Console.ReadLine();
        }
    }
}
