using PdfXenon.Standard;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Document document = new Document();
            document.Load(@"d:\Coffee.pdf");

            Console.ReadLine();
        }
    }
}
