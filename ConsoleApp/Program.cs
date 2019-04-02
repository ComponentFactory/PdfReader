using PdfXenon.Standard;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = new StreamReader(@"d:\Blank.pdf"))
            {
                Parser p = new Parser(reader.BaseStream);
                p.TestParse();
            }

            Console.ReadLine();
        }
    }
}
