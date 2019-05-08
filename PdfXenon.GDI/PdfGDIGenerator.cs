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
        private GraphicsPath _currentPath;
        private PdfPoint _currentPoint;        

        public Bitmap Bitmap { get; private set; }

        public override void Initialize(PdfRectangle mediaBox, PdfRectangle cropBox)
        {
            // Create a bitmap that is rounded up in size to the nearest pixel size
            Bitmap = new Bitmap((int)Math.Ceiling(mediaBox.UpperRight.X), (int)Math.Ceiling(mediaBox.UpperRight.Y), PixelFormat.Format32bppArgb);

            // Create a GDI+ context for writing to the bitmap
            _graphics = Graphics.FromImage(Bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.FillRectangle(Brushes.White, 0, 0, Bitmap.Width, Bitmap.Height);

            // Use a transform to convert from PDF (Y-axis points upwards) to GDI+ (Y-axis points downwards)
            _graphics.Transform = new Matrix(1, 0, 0, -1, 0, Bitmap.Height);

            // Set the initial clipping region to the cropbox
            GraphicsState.Clipping = new Region(new RectangleF(cropBox.LowerLeft.X, cropBox.LowerLeft.Y, cropBox.Width, cropBox.Height));
        }

        public override void SubPathStart(PdfPoint pt)
        {
            if (_currentPath == null)
                _currentPath = new GraphicsPath();
            else
                _currentPath.StartFigure();

            // Convert point to user space
            _currentPoint = GraphicsState.CTM.Transform(pt);
        }

        public override void SubPathLineTo(PdfPoint pt)
        {
            // Convert point to user space
            GraphicsState.CTM.Transform(pt);

            _currentPath.AddLine(_currentPoint.X, _currentPoint.Y, pt.X, pt.Y);
            _currentPoint = pt;
        }

        public override void SubPathBezier(PdfPoint pt2, PdfPoint pt3, PdfPoint pt4)
        {
            // Convert points to user space
            GraphicsState.CTM.Transform(pt2);
            GraphicsState.CTM.Transform(pt3);
            GraphicsState.CTM.Transform(pt4);

            _currentPath.AddBezier(_currentPoint.X, _currentPoint.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
            _currentPoint = pt4;
        }

        public override void SubPathClose()
        {
            _currentPath.CloseFigure();
        }

        public override void PathRectangle(PdfPoint pt, float width, float height)
        {
            if (_currentPath == null)
                _currentPath = new GraphicsPath();

            // Convert points to user space
            GraphicsState.CTM.Transform(pt);
            PdfPoint pt2 = GraphicsState.CTM.Transform(pt.X + width, pt.Y + height);

            _currentPath.AddRectangle(new RectangleF(pt.X, pt.Y, pt2.X - pt.X, pt2.Y - pt.Y));
        }

        public override void PathStroke()
        {
            using (Pen pen = CreatePen())
            {
                _graphics.Clip = (Region)GraphicsState.Clipping;
                _graphics.DrawPath(pen, _currentPath);
            }
        }

        public override void PathFill(bool evenOdd)
        {
            if (evenOdd)
                _currentPath.FillMode = FillMode.Alternate;
            else
                _currentPath.FillMode = FillMode.Winding;

            using (Brush brush = CreateBrush())
            {
                _graphics.Clip = (Region)GraphicsState.Clipping;
                _graphics.FillPath(brush, _currentPath);
            }
        }

        public override void PathClip(bool evenOdd)
        {
            if (evenOdd)
                _currentPath.FillMode = FillMode.Alternate;
            else
                _currentPath.FillMode = FillMode.Winding;

            // Always copy the region, so each graphics state has its own instance
            Region currentClip = (Region)GraphicsState.Clipping;
            Region newClip = currentClip.Clone();
            newClip.Intersect(_currentPath);
            GraphicsState.Clipping = newClip;
        }

        public override void PathEnd()
        {
            if (_currentPath != null)
            {
                _currentPath.Dispose();
                _currentPath = null;
            }
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

        private Brush CreateBrush()
        {
            // Get the current stroke colour and convert to GDI color
            PdfColorRGB rgb = GraphicsState.ColorSpaceNonStroking.ColorAsRGB();
            Color color = Color.FromArgb(255, (int)(255 * rgb.R), (int)(255 * rgb.G), (int)(255 * rgb.B));
            SolidBrush brush = new SolidBrush(color);

            return brush;
        }

        private Pen CreatePen()
        {
            // Get the current stroke colour and convert to GDI color
            PdfColorRGB rgb = GraphicsState.ColorSpaceStroking.ColorAsRGB();
            Color color = Color.FromArgb(255, (int)(255 * rgb.R), (int)(255 * rgb.G), (int)(255 * rgb.B));
            Pen pen = new Pen(color, GraphicsState.LineWidth);

            // Only if the dash pattern is more than a single value, do we need to apply it
            if ((GraphicsState.DashArray != null) && (GraphicsState.DashArray.Length > 0))
            {
                pen.DashStyle = DashStyle.Custom;
                pen.DashPattern = GraphicsState.DashArray;
                pen.DashOffset = GraphicsState.DashPhase;
            }

            // Define how the start and end caps of the line appear
            switch (GraphicsState.LineCapStyle)
            {
                case 0:
                default:
                    pen.StartCap = LineCap.Flat;
                    pen.EndCap = LineCap.Flat;
                    break;
                case 1:
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    break;
                case 2:
                    pen.StartCap = LineCap.Square;
                    pen.EndCap = LineCap.Square;
                    break;
            }

            // Define how multiple lines are joined together
            switch (GraphicsState.LineJoinStyle)
            {
                case 0:
                default:
                    pen.LineJoin = LineJoin.Miter;
                    break;
                case 1:
                    pen.LineJoin = LineJoin.Round;
                    break;
                case 2:
                    pen.LineJoin = LineJoin.Bevel;
                    break;
            }

            return pen;
        }
    }
}
