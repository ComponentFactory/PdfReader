using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfGraphicsState : PdfObject
    {
        private float? _lineWidth;
        private int? _lineCapStyle;
        private int? _lineJoinStyle;
        private float? _miterLength;
        private float[] _dashArray;
        private int? _dashPhase;
        private string _renderingIntent;
        private float? _flatness;
        private bool? _overPrint10;
        private bool? _overPrint13;
        private int? _overPrintMode;
        private float? _constantAlphaStroking;
        private float? _constantAlphaNonStroking;
        private float? _flatnessTolerance;
        private float? _smoothnessTolerance;
        private bool? _strokeAdjustment;
        private bool? _alphaSourceMask;
        private bool? _textKnockout;
        private PdfObject _blendMode;
        private PdfObject _font;
        private PdfObject _blackGeneration;
        private PdfObject _blackGeneration2;
        private PdfObject _undercolorRemoval;
        private PdfObject _undercolorRemoval2;
        private PdfObject _transfer;
        private PdfObject _transfer2;
        private PdfObject _halftone;
        private PdfObject _softMask;

        public PdfGraphicsState()
            : base(null)
        {
            // Default values for root graphics state
            _lineWidth = 1; 
            _lineCapStyle = 0; 
            _lineJoinStyle = 0; 
            _miterLength = 10;
            _dashArray = new float[] { };
            _dashPhase = 0;
            _renderingIntent = "RelativeColorimetric";
            _flatness = 0;
            _overPrint10 = false;
            _overPrint13 = false;
            _overPrintMode = 1;
            _blendMode = new PdfName(this, new ParseName("Normal"));
            _constantAlphaStroking = 1;
            _constantAlphaNonStroking = 1;
            _textKnockout = true;
        }

        public PdfGraphicsState(PdfGraphicsState parent)
            : base(parent)
        {
        }

        public PdfGraphicsState ParentGraphicsState { get => TypedParent<PdfGraphicsState>(); }

        public float LineWidth
        {
            get
            {
                if (_lineWidth.HasValue)
                    return _lineWidth.Value;
                else
                    return ParentGraphicsState.LineWidth;
            }

            set { _lineWidth = Math.Max(0, value); }
        }

        public int LineCapStyle
        {
            get
            {
                if (_lineCapStyle.HasValue)
                    return _lineCapStyle.Value;
                else
                    return ParentGraphicsState.LineCapStyle;
            }

            set { _lineCapStyle = Math.Max(0, value); }
        }

        public int LineJoinStyle
        {
            get
            {
                if (_lineJoinStyle.HasValue)
                    return _lineJoinStyle.Value;
                else
                    return ParentGraphicsState.LineJoinStyle;
            }

            set { _lineJoinStyle = Math.Max(0, value); }
        }

        public float MiterLength
        {
            get
            {
                if (_miterLength.HasValue)
                    return _miterLength.Value;
                else
                    return ParentGraphicsState.MiterLength;
            }

            set { _miterLength = value; }
        }

        public float[] DashArray
        {
            get
            {
                if (_dashArray != null)
                    return _dashArray;
                else
                    return ParentGraphicsState.DashArray;
            }

            set { _dashArray = value; }
        }

        public int DashPhase
        {
            get
            {
                if (_dashPhase.HasValue)
                    return _dashPhase.Value;
                else
                    return ParentGraphicsState.DashPhase;
            }

            set { _dashPhase = Math.Max(0, value); }
        }

        public string RenderingIntent
        {
            get
            {
                if (_renderingIntent != null)
                    return _renderingIntent;
                else
                    return ParentGraphicsState.RenderingIntent;
            }

            set { _renderingIntent = value; }
        }

        public float Flatness
        {
            get
            {
                if (_flatness.HasValue)
                    return _flatness.Value;
                else
                    return ParentGraphicsState.Flatness;
            }

            set { _flatness = Math.Min(100, Math.Max(0, value)); }
        }


        private int AsInteger(PdfObject obj)
        {
            if (obj is PdfInteger integer)
                return integer.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected an integer.");
        }

        private float AsAnyNumber(PdfObject obj)
        {
            if (obj is PdfInteger integer)
                return integer.Value;
            else if (obj is PdfReal real)
                return real.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a number.");
        }

        public bool? OverPrint10
        {
            get
            {
                if (_overPrint10.HasValue)
                    return _overPrint10.Value;
                else
                    return ParentGraphicsState.OverPrint10;
            }

            set { _overPrint10 = value; }
        }

        public bool? OverPrint13
        {
            get
            {
                if (_overPrint13.HasValue)
                    return _overPrint13.Value;
                else
                    return ParentGraphicsState.OverPrint13;
            }

            set { _overPrint13 = value; }
        }

        public int OverPrintMode
        {
            get
            {
                if (_overPrintMode.HasValue)
                    return _overPrintMode.Value;
                else
                    return ParentGraphicsState.OverPrintMode;
            }

            set { _overPrintMode = value; }
        }

        public float? ConstantAlphaStroking
        {
            get
            {
                if (_constantAlphaStroking.HasValue)
                    return _constantAlphaStroking.Value;
                else
                    return ParentGraphicsState.ConstantAlphaStroking;
            }

            set { _constantAlphaStroking = value; }
        }

        public float? ConstantAlphaNonStroking
        {
            get
            {
                if (_constantAlphaNonStroking.HasValue)
                    return _constantAlphaNonStroking.Value;
                else
                    return ParentGraphicsState.ConstantAlphaNonStroking;
            }

            set { _constantAlphaNonStroking = value; }
        }

        public float? FlatnessTolerance
        {
            get
            {
                if (_flatnessTolerance.HasValue)
                    return _flatnessTolerance.Value;
                else
                    return ParentGraphicsState.FlatnessTolerance;
            }

            set { _flatnessTolerance = value; }
        }

        public float? SmoothnessTolerance
        {
            get
            {
                if (_smoothnessTolerance.HasValue)
                    return _smoothnessTolerance.Value;
                else
                    return ParentGraphicsState.SmoothnessTolerance;
            }

            set { _smoothnessTolerance = value; }
        }

        public bool? StrokeAdjustment
        {
            get
            {
                if (_strokeAdjustment.HasValue)
                    return _strokeAdjustment.Value;
                else
                    return ParentGraphicsState.StrokeAdjustment;
            }

            set { _strokeAdjustment = value; }
        }

        public bool? AlphaSourceMask
        {
            get
            {
                if (_alphaSourceMask.HasValue)
                    return _alphaSourceMask.Value;
                else
                    return ParentGraphicsState.AlphaSourceMask;
            }

            set { _alphaSourceMask = value; }
        }

        public bool? TextKnockout
        {
            get
            {
                if (_textKnockout.HasValue)
                    return _textKnockout.Value;
                else
                    return ParentGraphicsState.TextKnockout;
            }

            set { _textKnockout = value; }
        }

        public PdfObject BlendMode
        {
            get
            {
                if (_blendMode != null)
                    return _blendMode;
                else
                    return ParentGraphicsState.BlendMode;
            }

            set { _blendMode = value; }
        }

        public PdfObject Font
        {
            get
            {
                if (_font != null)
                    return _font;
                else
                    return ParentGraphicsState.Font;
            }

            set { _font = value; }
        }

        public PdfObject BlackGeneration
        {
            get
            {
                if (_blackGeneration != null)
                    return _blackGeneration;
                else
                    return ParentGraphicsState.BlackGeneration;
            }

            set { _blackGeneration = value; }
        }

        public PdfObject BlackGeneration2
        {
            get
            {
                if (_blackGeneration2 != null)
                    return _blackGeneration2;
                else
                    return ParentGraphicsState.BlackGeneration2;
            }

            set { _blackGeneration2 = value; }
        }

        public PdfObject UndercolorRemoval
        {
            get
            {
                if (_undercolorRemoval != null)
                    return _undercolorRemoval;
                else
                    return ParentGraphicsState.UndercolorRemoval;
            }

            set { _undercolorRemoval = value; }
        }

        public PdfObject UndercolorRemoval2
        {
            get
            {
                if (_undercolorRemoval2 != null)
                    return _undercolorRemoval2;
                else
                    return ParentGraphicsState.UndercolorRemoval2;
            }

            set { _undercolorRemoval2 = value; }
        }

        public PdfObject Transfer
        {
            get
            {
                if (_transfer != null)
                    return _transfer;
                else
                    return ParentGraphicsState.Transfer;
            }

            set { _transfer = value; }
        }

        public PdfObject Transfer2
        {
            get
            {
                if (_transfer2 != null)
                    return _transfer2;
                else
                    return ParentGraphicsState.Transfer2;
            }

            set { _transfer2 = value; }
        }

        public PdfObject Halftone
        {
            get
            {
                if (_halftone != null)
                    return _halftone;
                else
                    return ParentGraphicsState.Halftone;
            }

            set { _halftone = value; }
        }

        public PdfObject SoftMask
        {
            get
            {
                if (_softMask != null)
                    return _softMask;
                else
                    return ParentGraphicsState.SoftMask;
            }

            set { _softMask = value; }
        }

        public object Clipping { get; set; }
    }
}
