using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfXenon.Standard;

namespace PdfXenon.GDI
{
    public class PdfGDIRenderer : PdfRenderer
    {
        private Graphics _graphics;

        public Bitmap Bitmap { get; private set; }

        public override void Initialize(PdfRectangle mediaBox, PdfRectangle cropBox)
        {
            // Create a bitmap that is rounded up in size to the nearest pixel size
            Bitmap = new Bitmap((int)Math.Ceiling(mediaBox.UpperRight.X), (int)Math.Ceiling(mediaBox.UpperRight.Y), PixelFormat.Format32bppArgb);

            // Create a GDI+ context for writing to the bitmap
            _graphics = Graphics.FromImage(Bitmap);
            _graphics.FillRectangle(Brushes.White, 0, 0, Bitmap.Width, Bitmap.Height);

            // Use a transform to convert from PDF (zero upwards) to GDI+ (zero downwards)
            _graphics.Transform = new Matrix(1, 0, 0, -1, 0, Bitmap.Height);

            // Set the initial clip region to the crop box
            _graphics.Clip = new Region(new RectangleF(cropBox.LowerLeft.X, cropBox.UpperRight.Y, cropBox.Width, cropBox.Height));
        }

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

        public override void Finshed()
        {
            if (_graphics != null)
            {
                _graphics.Flush();
                _graphics.Dispose();
                _graphics = null;
            }
        }
    }
}
