using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfProcessor : PdfObject
    {
        private PdfGraphicsState _graphicsState = new PdfGraphicsState();
        private Stack<PdfObject> _operands = new Stack<PdfObject>();

        public PdfProcessor(PdfObject parent)
            : base(parent)
        {
        }

        public abstract PdfDictionary Resources { get; }

        public void Process(PdfObject obj)
        {
            if (obj is PdfIdentifier identifer)
            {
                switch (identifer.Value)
                {
                    case "q": // Save graphics state
                        _graphicsState = new PdfGraphicsState(_graphicsState);
                        break;
                    case "Q": // Restore graphics state
                        _graphicsState = _graphicsState.ParentGraphicsState;
                        break;
                    case "w": // Set Line Width
                        _graphicsState.LineWidth = AsNumber(_operands.Pop());
                        break;
                    case "j": // Set Line Cap Style
                        _graphicsState.LineCapStyle = AsInteger(_operands.Pop());
                        break;
                    case "J": // Set Line Join Style
                        _graphicsState.LineJoinStyle = AsInteger(_operands.Pop());
                        break;
                    case "M": // Set Miter Length
                        _graphicsState.MiterLength = AsNumber(_operands.Pop());
                        break;
                    case "d": // Set Dash
                        _graphicsState.DashPhase = AsInteger(_operands.Pop());
                        _graphicsState.DashArray = AsNumberArray(_operands.Pop());
                        break;
                    case "ri": // Set Rendering Intent
                        _graphicsState.RenderingIntent = AsString(_operands.Pop());
                        break;
                    case "i": // Set Flatness
                        _graphicsState.Flatness = AsNumber(_operands.Pop());
                        break;
                    case "gs": // Set graphics state from dictionary
                        {
                            PdfDictionary extGStates = Resources.MandatoryValueRef<PdfDictionary>("ExtGState");
                            PdfDictionary extGState = extGStates.MandatoryValueRef<PdfDictionary>(AsString(_operands.Pop()));
                            ProcessExtGState(extGState);
                        }
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
                        _graphicsState.LineWidth = AsNumber(entry.Value);
                        break;
                    case "LC": // Set Line Cap Style
                        _graphicsState.LineCapStyle = AsInteger(entry.Value);
                        break;
                    case "LJ": // Set Line Join Style
                        _graphicsState.LineJoinStyle = AsInteger(entry.Value);
                        break;
                    case "ML": // Set Miter Length
                        _graphicsState.MiterLength = AsNumber(entry.Value);
                        break;
                    case "D": // Set Dash
                        {
                            List<PdfObject> array = AsArray(entry.Value);
                            _graphicsState.DashArray = AsNumberArray(array[0]);
                            _graphicsState.DashPhase = AsInteger(array[1]);
                        }
                        break;
                    case "RI": // Set Rendering Intent
                        _graphicsState.RenderingIntent = AsString(entry.Value);
                        break;
                    case "OP": // Set Over Print (available in all versions)
                        _graphicsState.OverPrint10 = AsBoolean(entry.Value);
                        break;
                    case "op": // Set Over Print (available in version 1.3 onwards)
                        _graphicsState.OverPrint13 = AsBoolean(entry.Value);
                        break;
                    case "OPM": // Set Overprint Mode
                        _graphicsState.OverPrintMode = AsInteger(entry.Value);
                        break;
                    case "BM": // Set Blend Mode
                        _graphicsState.BlendMode = entry.Value;
                        break;
                    case "CA": // Set Constant Alpha for Stroking
                        _graphicsState.ConstantAlphaStroking = AsNumber(entry.Value);
                        break;
                    case "ca": // Set Constant Alpha for Non-Stroking
                        _graphicsState.ConstantAlphaNonStroking = AsNumber(entry.Value);
                        break;
                    case "Font": // Font parameters
                        _graphicsState.Font = entry.Value;
                        break;
                    case "BG": // Set Black generation function
                        _graphicsState.BlackGeneration = entry.Value;
                        break;
                    case "BG2": // Set Black generation function or name
                        _graphicsState.BlackGeneration2 = entry.Value;
                        break;
                    case "UCR": // Set Undercolor removal function
                        _graphicsState.UndercolorRemoval = entry.Value;
                        break;
                    case "UCR2": // Set Undercolor removal function or name
                        _graphicsState.UndercolorRemoval2 = entry.Value;
                        break;
                    case "TR": // Set Transfer function
                        _graphicsState.Transfer = entry.Value;
                        break;
                    case "TR2": // Set Transfer function or name
                        _graphicsState.Transfer2 = entry.Value;
                        break;
                    case "HT": // Set Halftone
                        _graphicsState.Halftone = entry.Value;
                        break;
                    case "FL": // Set Flatness tolerance
                        _graphicsState.FlatnessTolerance = AsNumber(entry.Value);
                        break;
                    case "SM": // Set Smoothness tolerance
                        _graphicsState.SmoothnessTolerance = AsNumber(entry.Value);
                        break;
                    case "SA": // Set Stroke adjustment flag
                        _graphicsState.StrokeAdjustment = AsBoolean(entry.Value);
                        break;
                    case "SMask": // Set Soft Mask
                        _graphicsState.SoftMask = entry.Value;
                        break;
                    case "AIS": // Set Alpha Source Mask
                        _graphicsState.AlphaSourceMask = AsBoolean(entry.Value);
                        break;
                    case "TK": // Set Text Knockout flag
                        _graphicsState.TextKnockout = AsBoolean(entry.Value);
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

        private double AsNumber(PdfObject obj)
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

        private double[] AsNumberArray(PdfObject obj)
        {
            if (obj is PdfArray array)
            {
                List<double> numbers = new List<double>();
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
