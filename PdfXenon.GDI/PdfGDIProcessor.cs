using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfXenon.Standard;

namespace PdfXenon.GDI
{
    public class PdfGDIProcessor : PdfProcessor
    {
        public override void SubPathStart(PdfPoint pt)
        {
        }

        public override void SubPathLineTo(PdfPoint pt)
        {
        }

        public override void SubPathBezier(PdfPoint pt1, PdfPoint pt2, PdfPoint pt3)
        {
        }

        public override void SubPathClose()
        {
        }

        public override void PathRectangle(PdfPoint pt1, float width, float height)
        {
        }

        public override void PathStroke()
        {
        }

        public override void PathFill(bool evenOdd)
        {
        }

        public override void PathClip(bool evenOdd)
        {
        }

        public override void PathEnd()
        {
        }
    }
}
