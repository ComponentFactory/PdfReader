using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfProcessor
    {
        private Stack<PdfObject> _operands = new Stack<PdfObject>();

        public PdfProcessor()
        {
            GraphicsState = new PdfGraphicsState();
        }

        public PdfDictionary Resources { get; set; }
        public PdfGraphicsState GraphicsState { get; set; }

        public abstract void SubPathStart(PdfPoint pt);
        public abstract void SubPathLineTo(PdfPoint pt);
        public abstract void SubPathBezier(PdfPoint pt1, PdfPoint pt2, PdfPoint pt3);
        public abstract void SubPathClose();
        public abstract void PathRectangle(PdfPoint pt1, float width, float height);
        public abstract void PathStroke();
        public abstract void PathFill(bool evenOdd);
        public abstract void PathClip(bool evenOdd);
        public abstract void PathEnd();

        public void Process(PdfObject obj)
        {
            if (obj is PdfIdentifier identifer)
            {
                switch (identifer.Value)
                {
                    case "q": // Save graphics state
                        GraphicsState = new PdfGraphicsState(GraphicsState);
                        break;
                    case "Q": // Restore graphics state
                        GraphicsState = GraphicsState.ParentGraphicsState;
                        break;
                    case "w": // Set Line Width
                        GraphicsState.LineWidth = AsNumber(_operands.Pop());
                        break;
                    case "j": // Set Line Cap Style
                        GraphicsState.LineCapStyle = AsInteger(_operands.Pop());
                        break;
                    case "J": // Set Line Join Style
                        GraphicsState.LineJoinStyle = AsInteger(_operands.Pop());
                        break;
                    case "M": // Set Miter Length
                        GraphicsState.MiterLength = AsNumber(_operands.Pop());
                        break;
                    case "d": // Set Dash
                        GraphicsState.DashPhase = AsInteger(_operands.Pop());
                        GraphicsState.DashArray = AsNumberArray(_operands.Pop());
                        break;
                    case "ri": // Set Rendering Intent
                        GraphicsState.RenderingIntent = AsString(_operands.Pop());
                        break;
                    case "i": // Set Flatness
                        GraphicsState.Flatness = AsNumber(_operands.Pop());
                        break;
                    case "gs": // Set graphics state from dictionary
                        {
                            PdfDictionary extGStates = Resources.MandatoryValueRef<PdfDictionary>("ExtGState");
                            PdfDictionary extGState = extGStates.MandatoryValueRef<PdfDictionary>(AsString(_operands.Pop()));
                            ProcessExtGState(extGState);
                        }
                        break;
                    case "m": // Start a new subpath and set currrent point
                        {
                            float y = AsNumber(_operands.Pop());
                            SubPathStart(new PdfPoint(AsNumber(_operands.Pop()), y));
                        }
                        break;
                    case "l": // Append straight line segment to subpath
                        {
                            float y = AsNumber(_operands.Pop());
                            SubPathLineTo(new PdfPoint(AsNumber(_operands.Pop()), y));
                        }
                        break;
                    case "c": // Append cubic Bezier curve to subpath
                        {
                            float y3 = AsNumber(_operands.Pop());
                            PdfPoint pt3 = new PdfPoint(AsNumber(_operands.Pop()), y3);
                            float y2 = AsNumber(_operands.Pop());
                            PdfPoint pt2 = new PdfPoint(AsNumber(_operands.Pop()), y2);
                            float y1 = AsNumber(_operands.Pop());
                            PdfPoint pt1 = new PdfPoint(AsNumber(_operands.Pop()), y1);
                            SubPathBezier(pt1, pt2, pt3);
                        }
                        break;
                    case "v": // Append cubic Bezier curve to subpath (first control point is same as initial point)
                        {
                            float y3 = AsNumber(_operands.Pop());
                            PdfPoint pt3 = new PdfPoint(AsNumber(_operands.Pop()), y3);
                            float y2 = AsNumber(_operands.Pop());
                            PdfPoint pt2 = new PdfPoint(AsNumber(_operands.Pop()), y2);
                            SubPathBezier(pt2, pt2, pt3);
                        }
                        break;
                    case "y": // Append cubic Bezier curve to subpath (second control point is same as final point)
                        {
                            float y3 = AsNumber(_operands.Pop());
                            PdfPoint pt3 = new PdfPoint(AsNumber(_operands.Pop()), y3);
                            float y1 = AsNumber(_operands.Pop());
                            PdfPoint pt1 = new PdfPoint(AsNumber(_operands.Pop()), y1);
                            SubPathBezier(pt1, pt1, pt3);
                        }
                        break;
                    case "h": // Close subpath
                        SubPathClose();
                        break;
                    case "re": // Append rectange as complete subpath
                        {
                            float h = AsNumber(_operands.Pop());
                            float w = AsNumber(_operands.Pop());
                            float y = AsNumber(_operands.Pop());
                            PdfPoint pt = new PdfPoint(AsNumber(_operands.Pop()), y);
                            PathRectangle(pt, w, h);
                        }
                        break;
                    case "S": // Stroke the path
                        PathStroke();
                        PathEnd();
                        break;
                    case "s": // Close and Stroke the path
                        SubPathClose();
                        PathStroke();
                        PathEnd();
                        break;
                    case "f": // Fill the path
                    case "F": // Fill the path (used in older versions, does same as 'f')
                        PathFill(false);
                        PathEnd();
                        break;
                    case "f*": // Fill the path using the even-odd rule
                        PathFill(true);
                        PathEnd();
                        break;
                    case "B": // Fill and Stroke the path
                        PathFill(false);
                        PathStroke();
                        PathEnd();
                        break;
                    case "B*": // Fill and Stroke the path using the even-odd rule
                        PathFill(true);
                        PathStroke();
                        PathEnd();
                        break;
                    case "b": // Close, Fill and Stroke the path
                        SubPathClose();
                        PathFill(false);
                        PathStroke();
                        PathEnd();
                        break;
                    case "b*": // Close, Fill and Stroke the path using the even-odd rule
                        SubPathClose();
                        PathFill(true);
                        PathStroke();
                        PathEnd();
                        break;
                    case "n": // End the path without filling or stroking it
                        PathEnd();
                        break;
                    case "W": // Update clipping path when current path is ended
                        PathClip(false);
                        break;
                    case "W*": // Update clipping path when current path is ended using the even-odd rule
                        PathClip(true);
                        break;
                    default:
                        // Ignore anything we do not recognize
                        break;
                }
            }
            else if (obj is PdfName name)
            {
                switch (name.Value)
                {
                    default:
                        // If not an identifier that happens to match a Name, then must be an operand
                        _operands.Push(name);
                        break;
                }
            }

            // Any other type must be an operand
            _operands.Push(obj);
        }

        public void ProcessExtGState(PdfDictionary dictionary)
        {
            foreach (var entry in dictionary)
            {
                switch (entry.Key)
                {
                    case "LW": // Set Line Width
                        GraphicsState.LineWidth = AsNumber(entry.Value);
                        break;
                    case "LC": // Set Line Cap Style
                        GraphicsState.LineCapStyle = AsInteger(entry.Value);
                        break;
                    case "LJ": // Set Line Join Style
                        GraphicsState.LineJoinStyle = AsInteger(entry.Value);
                        break;
                    case "ML": // Set Miter Length
                        GraphicsState.MiterLength = AsNumber(entry.Value);
                        break;
                    case "D": // Set Dash
                        {
                            List<PdfObject> array = AsArray(entry.Value);
                            GraphicsState.DashArray = AsNumberArray(array[0]);
                            GraphicsState.DashPhase = AsInteger(array[1]);
                        }
                        break;
                    case "RI": // Set Rendering Intent
                        GraphicsState.RenderingIntent = AsString(entry.Value);
                        break;
                    case "OP": // Set Over Print (available in all versions)
                        GraphicsState.OverPrint10 = AsBoolean(entry.Value);
                        break;
                    case "op": // Set Over Print (available in version 1.3 onwards)
                        GraphicsState.OverPrint13 = AsBoolean(entry.Value);
                        break;
                    case "OPM": // Set Overprint Mode
                        GraphicsState.OverPrintMode = AsInteger(entry.Value);
                        break;
                    case "BM": // Set Blend Mode
                        GraphicsState.BlendMode = entry.Value;
                        break;
                    case "CA": // Set Constant Alpha for Stroking
                        GraphicsState.ConstantAlphaStroking = AsNumber(entry.Value);
                        break;
                    case "ca": // Set Constant Alpha for Non-Stroking
                        GraphicsState.ConstantAlphaNonStroking = AsNumber(entry.Value);
                        break;
                    case "Font": // Font parameters
                        GraphicsState.Font = entry.Value;
                        break;
                    case "BG": // Set Black generation function
                        GraphicsState.BlackGeneration = entry.Value;
                        break;
                    case "BG2": // Set Black generation function or name
                        GraphicsState.BlackGeneration2 = entry.Value;
                        break;
                    case "UCR": // Set Undercolor removal function
                        GraphicsState.UndercolorRemoval = entry.Value;
                        break;
                    case "UCR2": // Set Undercolor removal function or name
                        GraphicsState.UndercolorRemoval2 = entry.Value;
                        break;
                    case "TR": // Set Transfer function
                        GraphicsState.Transfer = entry.Value;
                        break;
                    case "TR2": // Set Transfer function or name
                        GraphicsState.Transfer2 = entry.Value;
                        break;
                    case "HT": // Set Halftone
                        GraphicsState.Halftone = entry.Value;
                        break;
                    case "FL": // Set Flatness tolerance
                        GraphicsState.FlatnessTolerance = AsNumber(entry.Value);
                        break;
                    case "SM": // Set Smoothness tolerance
                        GraphicsState.SmoothnessTolerance = AsNumber(entry.Value);
                        break;
                    case "SA": // Set Stroke adjustment flag
                        GraphicsState.StrokeAdjustment = AsBoolean(entry.Value);
                        break;
                    case "SMask": // Set Soft Mask
                        GraphicsState.SoftMask = entry.Value;
                        break;
                    case "AIS": // Set Alpha Source Mask
                        GraphicsState.AlphaSourceMask = AsBoolean(entry.Value);
                        break;
                    case "TK": // Set Text Knockout flag
                        GraphicsState.TextKnockout = AsBoolean(entry.Value);
                        break;
                    default:
                        // Ignore anything we do not recognize
                        break;
                }
            }
        }

        private bool AsBoolean(PdfObject obj)
        {
            if (obj is PdfBoolean boolean)
                return boolean.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a boolean.");
        }

        private string AsString(PdfObject obj)
        {
            if (obj is PdfName name)
                return name.Value;
            else if (obj is PdfString str)
                return str.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a string.");
        }

        private int AsInteger(PdfObject obj)
        {
            if (obj is PdfInteger integer)
                return integer.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected an integer.");
        }

        private float AsNumber(PdfObject obj)
        {
            if (obj is PdfInteger integer)
                return integer.Value;
            else if (obj is PdfReal real)
                return real.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a number.");
        }

        private List<PdfObject> AsArray(PdfObject obj)
        {
            if (obj is PdfArray array)
                return array.Objects;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected an integer array.");
        }

        private float[] AsNumberArray(PdfObject obj)
        {
            if (obj is PdfArray array)
            {
                List<float> numbers = new List<float>();
                foreach(PdfObject item in array.Objects)
                {
                    if (item is PdfInteger integer)
                        numbers.Add(integer.Value);
                    else if (item is PdfReal real)
                        numbers.Add(real.Value);
                    else
                        throw new ApplicationException($"Array contains object of type '{obj.GetType().Name}', expected only numbers.");

                }

                return numbers.ToArray();
            }

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected an integer array.");
        }
    }
}
