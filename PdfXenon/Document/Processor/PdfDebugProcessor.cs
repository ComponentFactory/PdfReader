using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDebugProcessor : PdfProcessor
    {
        public PdfDebugProcessor()
        {
        }

        public override void SubPathStart(PdfPoint pt)
        {
            Console.WriteLine($"SubPathStart {pt}");
        }

        public override void SubPathLineTo(PdfPoint pt)
        {
            Console.WriteLine($"SubPathLineTo {pt}");
        }

        public override void SubPathBezier(PdfPoint pt1, PdfPoint pt2, PdfPoint pt3)
        {
            Console.WriteLine($"SubPathBezier {pt1} {pt2} {pt3}");
        }

        public override void SubPathClose()
        {
            Console.WriteLine($"SubPathClose");
        }

        public override void PathRectangle(PdfPoint pt, float width, float height)
        {
            Console.WriteLine($"PathRectangle {pt} ({width},{height})");
        }

        public override void PathStroke()
        {
            Console.WriteLine($"PathStroke");
        }

        public override void PathFill(bool evenOdd)
        {
            Console.WriteLine($"PathFill EvenOdd:{evenOdd}");
        }

        public override void PathClip(bool evenOdd)
        {
            Console.WriteLine($"PathClip EvenOdd:{evenOdd}");
        }

        public override void PathEnd()
        {
            Console.WriteLine($"PathEnd");
        }
    }
}
