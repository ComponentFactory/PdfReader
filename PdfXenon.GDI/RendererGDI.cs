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
    public class RendererGDI : Renderer
    {
        private Graphics _graphics;
        private GraphicsPath _currentPath;
        private RenderPoint _currentPoint;        

        public Bitmap Bitmap { get; private set; }

        public override void Initialize(PdfRectangle mediaBox, PdfRectangle cropBox)
        {
            // Create a bitmap that is rounded up in size to the nearest pixel size
            Bitmap = new Bitmap((int)Math.Ceiling(mediaBox.UpperRightX), (int)Math.Ceiling(mediaBox.UpperRightY), PixelFormat.Format32bppArgb);

            // Create a GDI+ context for writing to the bitmap
            _graphics = Graphics.FromImage(Bitmap);
            _graphics.SmoothingMode = SmoothingMode.AntiAlias;
            _graphics.FillRectangle(Brushes.White, 0, 0, Bitmap.Width, Bitmap.Height);

            // Use a transform to convert from PDF (Y-axis points upwards) to GDI+ (Y-axis points downwards)
            _graphics.Transform = new Matrix(1, 0, 0, -1, 0, Bitmap.Height);

            // Set the initial clipping region to the cropbox
            GraphicsState.Clipping = new Region(new RectangleF(cropBox.LowerLeftX, cropBox.LowerLeftY, cropBox.Width, cropBox.Height));
        }

        public override void SubPathStart(RenderPoint pt)
        {
            if (_currentPath == null)
                _currentPath = new GraphicsPath();
            else
                _currentPath.StartFigure();

            // Convert point to user space
            _currentPoint = GraphicsState.CTM.Transform(pt);
        }

        public override void SubPathLineTo(RenderPoint pt)
        {
            // Convert point to user space
            GraphicsState.CTM.Transform(pt);

            _currentPath.AddLine(_currentPoint.X, _currentPoint.Y, pt.X, pt.Y);
            _currentPoint = pt;
        }

        public override void SubPathBezier(RenderPoint pt2, RenderPoint pt3, RenderPoint pt4)
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

        public override void PathRectangle(RenderPoint pt, float width, float height)
        {
            if (_currentPath == null)
                _currentPath = new GraphicsPath();
            else
                _currentPath.StartFigure();

            // Convert to user space
            RenderPoint pt1 = GraphicsState.CTM.Transform(pt.X, pt.Y);
            RenderPoint pt2 = GraphicsState.CTM.Transform(pt.X + width, pt.Y + height);

            _currentPath.AddLine(pt1.X, pt1.Y, pt2.X, pt1.Y);
            _currentPath.AddLine(pt2.X, pt1.Y, pt2.X, pt2.Y);
            _currentPath.AddLine(pt2.X, pt2.Y, pt1.X, pt2.Y);

            _currentPath.CloseFigure();
        }

        public override void PathStroke()
        {
            using (DrawingResource resource = CreateStrokingPen().Apply(this))
            {
                _graphics.Clip = (Region)GraphicsState.Clipping;
                _graphics.DrawPath(resource.Pen, _currentPath);
            }
        }

        public override void PathFill(bool evenOdd)
        {
            if (evenOdd)
                _currentPath.FillMode = FillMode.Alternate;
            else
                _currentPath.FillMode = FillMode.Winding;

            using (DrawingResource resource = CreateNonStrokingBrush().Apply(this))
            {
                _graphics.Clip = (Region)GraphicsState.Clipping;

                if (resource.Background != null)
                    _graphics.FillPath(resource.Background, _currentPath);

                _graphics.FillPath(resource.Brush, _currentPath);
            }
        }

        public override void PathShading(RenderPatternShading shading)
        {
            Region clipping = (Region)GraphicsState.Clipping;
            RectangleF clippingBounds = clipping.GetBounds(_graphics);
            GraphicsPath clippingPath = new GraphicsPath();
            clippingPath.AddRectangle(clippingBounds);

            // Take the unit sized shading and update using the matrix so it maps to the clipping area
            float distance = (clippingBounds.Width + clippingBounds.Height) / 2;
            List<ParseObject> matrixValues = new List<ParseObject>()
            {
                new ParseReal(distance), new ParseReal(0f),
                new ParseReal(0f), new ParseReal(distance),
                new ParseReal(clippingBounds.X + (clippingBounds.Width / 2)),
                new ParseReal(clippingBounds.Y + (clippingBounds.Height / 2))
            };
            shading.Matrix = new PdfArray(null, new ParseArray(matrixValues));

            using (DrawingResource resource = CreateNonStrokingBrushFromShading(shading).Apply(this))
            {
                _graphics.Clip = clipping;
                _graphics.FillPath(resource.Brush, clippingPath);
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

        private DrawingResource CreateNonStrokingBrush()
        {
            if (GraphicsState.ColorSpaceNonStroking is RenderColorSpaceRGB colorSpaceRGB)
                return CreateNonStrokingBrushFromRGB(colorSpaceRGB);
            else if (GraphicsState.ColorSpaceNonStroking is RenderColorSpacePattern colorSpacePatten)
                return CreateNonStrokingBrushFromPattern(colorSpacePatten);

            throw new NotImplementedException($"Colorspace type '{GraphicsState.ColorSpaceNonStroking.GetType().Name}' not implemented.");
        }

        private DrawingResource CreateNonStrokingBrushFromRGB(RenderColorSpaceRGB colorSpaceRGB)
        {
            RenderColorRGB rgb = colorSpaceRGB.GetColorRGB();

            SolidBrush brush = new SolidBrush(Color.FromArgb((int)(255 * GraphicsState.ConstantAlphaNonStroking), 
                                                             (int)(255 * rgb.R), 
                                                             (int)(255 * rgb.G), 
                                                             (int)(255 * rgb.B)));

            return new DrawingResource() { Brush = brush };
        }

        private DrawingResource CreateNonStrokingBrushFromPattern(RenderColorSpacePattern colorSpacePatten)
        {
            RenderPatternType pattern = colorSpacePatten.GetPattern();
            if (pattern is RenderPatternShading shading)
                return CreateNonStrokingBrushFromShading(shading);

            throw new NotImplementedException($"Pattern type '{pattern.GetType().Name}' not implemented.");
        }

        private DrawingResource CreateNonStrokingBrushFromShading(RenderPatternShading shading)
        {
            if (shading is RenderPatternShadingAxial axial)
            {
                // Get the points that represents the start and end of the axial gradient line
                PdfArray coord = axial.Coords;
                PointF pointStart = new PointF(coord.Objects[0].AsNumber(), coord.Objects[1].AsNumber());
                PointF pointEnd = new PointF(coord.Objects[2].AsNumber(), coord.Objects[3].AsNumber());

                // Default the colors to white and black, they are overridden later with actual colors
                LinearGradientBrush brush = new LinearGradientBrush(pointStart, pointEnd, Color.White, Color.Black);

                // Get the color space, needed to convert the function result to RGB color values
                RenderColorSpaceRGB colorSpace = (RenderColorSpaceRGB)axial.ColorSpaceValue;

                // The more positions we provide, the more accurate the gradient becomes
                Color[] colors = new Color[32];
                float[] positions = new float[32];
                for (int i = 0; i < positions.Length; i++)
                {
                    // Ensure that the first and last positions are exactly 0 and 1
                    float position = 0f;
                    if (i == (positions.Length - 1))
                        position = 1f;
                    else
                        position = 1f / positions.Length * i;

                    // Use the function to get values for the position, then use the color space to convert that result that into actual RGB values
                    colorSpace.Parse(axial.FunctionValue.Call(new float[] { position }));
                    RenderColorRGB rgb = colorSpace.GetColorRGB();

                    colors[i] = Color.FromArgb((int)(255 * GraphicsState.ConstantAlphaNonStroking), (int)(255 * rgb.R), (int)(255 * rgb.G), (int)(255 * rgb.B));
                    positions[i] = position;
                }

                brush.InterpolationColors = new ColorBlend()
                {
                    Colors = colors,
                    Positions = positions
                };

                Brush backgroundBrush = null;
                if (axial.Background != null)
                {
                    colorSpace.Parse(axial.Background.AsNumberArray());
                    RenderColorRGB rgb = colorSpace.GetColorRGB();
                    backgroundBrush = new SolidBrush(Color.FromArgb((int)(255 * GraphicsState.ConstantAlphaNonStroking), (int)(255 * rgb.R), (int)(255 * rgb.G), (int)(255 * rgb.B)));
                }

                // Make sure we apply any optional dictionary changes in pushed graphics state
                return new DrawingResource() { Brush = brush, PushPop = true, ExtGState = axial.ExtGState, Background = backgroundBrush };
            }
            else if (shading is RenderPatternShadingRadial radial)
            {
                // Get the points that represents the two circles
                PdfArray coordArray = radial.Coords;
                float[] coords = coordArray.AsNumberArray();

                if (radial.Matrix != null)
                {
                    // Apply the matrix to the coordinates
                    RenderMatrix matrix = new RenderMatrix(radial.Matrix.AsNumberArray());
                    RenderPoint t1 = matrix.Transform(coords[0], coords[1]);
                    RenderPoint t2 = matrix.Transform(coords[0] + coords[2], coords[1]);
                    coords[0] = t1.X;
                    coords[1] = t1.Y;
                    coords[2] = t1.Distance(t2);
                    t1 = matrix.Transform(coords[3], coords[4]);
                    t2 = matrix.Transform(coords[3] + coords[5], coords[4]);
                    coords[3] = t1.X;
                    coords[4] = t1.Y;
                    coords[5] = t1.Distance(t2);
                }

                // Create a path that represents the two circles, but ignore empty circles
                GraphicsPath path = new GraphicsPath();

                // We only add a circle that has an actual size
                if (coords[2] != 0) path.AddEllipse(coords[0] - coords[2], coords[1] - coords[2], coords[2] * 2, coords[2] * 2);
                if (coords[5] != 0) path.AddEllipse(coords[3] - coords[5], coords[4] - coords[5], coords[5] * 2, coords[5] * 2);

                PathGradientBrush brush = new PathGradientBrush(path);

                // A zero sized circle identifies the center point that color start from
                if (coords[2] == 0) brush.CenterPoint = new PointF(coords[0], coords[1]);
                if (coords[5] == 0) brush.CenterPoint = new PointF(coords[3], coords[4]);

                //// Get the color space, needed to convert the function result to RGB color values
                RenderColorSpaceRGB colorSpace = (RenderColorSpaceRGB)radial.ColorSpaceValue;

                // The more positions we provide, the more accurate the gradient becomes
                Color[] colors = new Color[32];
                float[] positions = new float[32];
                for (int i = 0; i < positions.Length; i++)
                {
                    // Ensure that the first and last positions are exactly 0 and 1
                    float position = 0f;
                    if (i == (positions.Length - 1))
                        position = 1f;
                    else
                        position = 1f / positions.Length * i;

                    // If the first circle is the center point then invert the color ordering
                    float funcPosition = (coords[2] == 0) ? 1f - position : position;
                    colorSpace.Parse(radial.FunctionValue.Call(new float[] { funcPosition }));
                    RenderColorRGB rgb = colorSpace.GetColorRGB();

                    colors[i] = Color.FromArgb((int)(255 * GraphicsState.ConstantAlphaNonStroking), (int)(255 * rgb.R), (int)(255 * rgb.G), (int)(255 * rgb.B));
                    positions[i] = position;
                }

                brush.InterpolationColors = new ColorBlend()
                {
                    Colors = colors,
                    Positions = positions
                };

                Brush backgroundBrush = null;
                if (radial.Background != null)
                {
                    colorSpace.Parse(radial.Background.AsNumberArray());
                    RenderColorRGB rgb = colorSpace.GetColorRGB();
                    backgroundBrush = new SolidBrush(Color.FromArgb((int)(255 * GraphicsState.ConstantAlphaNonStroking), (int)(255 * rgb.R), (int)(255 * rgb.G), (int)(255 * rgb.B)));
                }

                // Make sure we apply any optional dictionary changes in pushed graphics state
                return new DrawingResource() { Brush = brush, PushPop = true, ExtGState = radial.ExtGState, Background = backgroundBrush };
            }

            throw new NotImplementedException($"Shading type '{shading.GetType().Name}' not implemented.");
        }

        private DrawingResource CreateStrokingPen()
        {
            if (GraphicsState.ColorSpaceStroking is RenderColorSpaceRGB colorSpaceRGB)
                return CreateStrokingPenFromRGB(colorSpaceRGB);

            throw new NotImplementedException($"Colorspace type '{GraphicsState.ColorSpaceStroking.GetType().Name}' not implemented.");
        }

        private DrawingResource CreateStrokingPenFromRGB(RenderColorSpaceRGB colorSpaceRGB)
        {
            // Get the current stroke colour and convert to GDI color
            RenderColorRGB rgb = colorSpaceRGB.GetColorRGB();

            Color color = Color.FromArgb((int)(255 * GraphicsState.ConstantAlphaStroking), 
                                         (int)(255 * rgb.R), 
                                         (int)(255 * rgb.G), 
                                         (int)(255 * rgb.B));

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

            return new DrawingResource() { Pen = pen };
        }

        public class DrawingResource : IDisposable
        {
            public Pen Pen { get; set; }
            public Brush Brush { get; set; }
            public Renderer Renderer { get; set; }
            public bool PushPop { get; set; }
            public PdfDictionary ExtGState { get; set; }
            public Brush Background { get; set; }

            public DrawingResource Apply(Renderer renderer)
            {
                Renderer = renderer;

                if (PushPop)
                    Renderer.PushGraphicsState();

                if (ExtGState != null)
                    Renderer.ProcessExtGState(ExtGState);

                return this;
            }

            public void Dispose()
            {
                if (PushPop)
                    Renderer.PopGraphicsState();

                if (Pen != null)
                {
                    Pen.Dispose();
                    Pen = null;
                }

                if (Brush != null)
                {
                    Brush.Dispose();
                    Brush = null;
                }

                if (Background != null)
                {
                    Background.Dispose();
                    Background = null;
                }
            }
        }
    }
}
