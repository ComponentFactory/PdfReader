using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfProcessor : PdfObject
    {
        private PdfGraphicsState _currentState = new PdfGraphicsState();
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
                        _currentState = new PdfGraphicsState(_currentState);
                        break;
                    case "Q": // Restore graphics state
                        _currentState = _currentState.ParentGraphicsState;
                        break;
                    case "w": // Set Line Width
                        _currentState.LineWidth = AsNumber(_operands.Pop());
                        break;
                    case "j": // Set Line Cap Style
                        _currentState.LineCapStyle = AsInteger(_operands.Pop());
                        break;
                    case "J": // Set Line Join Style
                        _currentState.LineJoinStyle = AsInteger(_operands.Pop());
                        break;
                    case "M": // Set Miter Length
                        _currentState.MiterLength = AsNumber(_operands.Pop());
                        break;
                    case "d": // Set Dash
                        _currentState.DashPhase = AsInteger(_operands.Pop());
                        _currentState.DashArray = AsNumberArray(_operands.Pop());
                        break;
                    case "ri": // Set Rendering Intent
                        _currentState.RenderingIntent = AsString(_operands.Pop());
                        break;
                    case "i": // Set Flatness
                        _currentState.Flatness = AsNumber(_operands.Pop());
                        break;
                    case "gs": // Set parameters from graphics dictionary
                        string dictName = AsString(_operands.Pop());
                        PdfDictionary extGStates = Resources.MandatoryValueRef<PdfDictionary>("ExtGState");
                        PdfDictionary extGState = extGStates.MandatoryValueRef<PdfDictionary>(dictName);
                        UpdateFromExtGState(extGState);
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
            else
            {
                // Assume it must be an operand
                _operands.Push(obj);
            }
        }

        public void UpdateFromExtGState(PdfDictionary dictionary)
        {
            foreach (var entry in dictionary)
            {
                switch (entry.Key)
                {
                    case "LW": // Set Line Width
                        _currentState.LineWidth = AsNumber(entry.Value);
                        break;
                    case "LC": // Set Line Cap Style
                        _currentState.LineCapStyle = AsInteger(entry.Value);
                        break;
                    case "LJ": // Set Line Join Style
                        _currentState.LineJoinStyle = AsInteger(entry.Value);
                        break;
                    case "ML": // Set Miter Length
                        _currentState.MiterLength = AsNumber(entry.Value);
                        break;
                    case "D": // Set Dash
                        List<PdfObject> array = AsArray(entry.Value);
                        _currentState.DashArray = AsNumberArray(array[0]);
                        _currentState.DashPhase = AsInteger(array[1]);
                        break;
                    case "RI": // Set Rendering Intent
                        _currentState.RenderingIntent = AsString(entry.Value);
                        break;
                    case "OP": // Set Over Print (available in all versions)
                        _currentState.OverPrint10 = AsBoolean(entry.Value);
                        break;
                    case "op": // Set Over Print (available in version 1.3 onwards for everything except stroking)
                        _currentState.OverPrint13 = AsBoolean(entry.Value);
                        break;
                    case "OPM": // Set Overprint Mode
                        _currentState.OverPrintMode = AsInteger(entry.Value);
                        break;
                    case "BM": // Set Blend Mode
                        _currentState.BlendMode = entry.Value;
                        break;
                    case "CA": // Set Constant Alpha for Stroking
                        _currentState.ConstantAlphaStroking = AsNumber(entry.Value);
                        break;
                    case "ca": // Set Constant Alpha for Non-Stroking
                        _currentState.ConstantAlphaNonStroking = AsNumber(entry.Value);
                        break;
                    case "Font": // Font parameters
                        _currentState.Font = entry.Value;
                        break;
                    case "BG": // Set Black generation function
                        _currentState.BlackGeneration = entry.Value;
                        break;
                    case "BG2": // Set Black generation function or name
                        _currentState.BlackGeneration2 = entry.Value;
                        break;
                    case "UCR": // Set Undercolor removal function
                        _currentState.UndercolorRemoval = entry.Value;
                        break;
                    case "UCR2": // Set Undercolor removal function or name
                        _currentState.UndercolorRemoval2 = entry.Value;
                        break;
                    case "TR": // Set Transfer function
                        _currentState.Transfer = entry.Value;
                        break;
                    case "TR2": // Set Transfer function or name
                        _currentState.Transfer2 = entry.Value;
                        break;
                    case "HT": // Set Halftone
                        _currentState.Halftone = entry.Value;
                        break;
                    case "FL": // Set Flatness tolerance
                        _currentState.FlatnessTolerance = AsNumber(entry.Value);
                        break;
                    case "SM": // Set Smoothness tolerance
                        _currentState.SmoothnessTolerance = AsNumber(entry.Value);
                        break;
                    case "SA": // Set Stroke adjustment flag
                        _currentState.StrokeAdjustment = AsBoolean(entry.Value);
                        break;
                    case "SMask": // Set Soft Mask
                        _currentState.SoftMask = entry.Value;
                        break;
                    case "AIS": // Set Alpha Source Mask
                        _currentState.AlphaSourceMask = AsBoolean(entry.Value);
                        break;
                    case "TK": // Set Text Knockout flag
                        _currentState.TextKnockout = AsBoolean(entry.Value);
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
