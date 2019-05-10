using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderGraphicsState : RenderObject
    {
        private RenderColorSpace _colorSpaceStroking;
        private RenderColorSpace _colorSpaceNonStroking;
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
        private object _clip;

        public RenderGraphicsState(RenderObject parent, bool init = false)
            : base(parent)
        {
            if (init)
            {
                // Default values for root graphics state
                LocalCTM = new RenderMatrix();
                LocalLineWidth = 1;
                LocalLineCapStyle = 0;
                LocalLineJoinStyle = 0;
                LocalMiterLength = 10;
                LocalDashArray = new float[] { };
                LocalDashPhase = 0;
                LocalRenderingIntent = "RelativeColorimetric";
                LocalFlatness = 0;
                LocalOverPrint10 = false;
                LocalOverPrint13 = false;
                LocalOverPrintMode = 1;
                LocalConstantAlphaStroking = 1;
                LocalConstantAlphaNonStroking = 1;
                LocalTextKnockout = true;
            }
        }

        public override void Visit(IRenderObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public RenderGraphicsState ParentGraphicsState { get => TypedParent<RenderGraphicsState>(); }

        public int Depth
        {
            get
            {
                int depth = 1;

                RenderGraphicsState parent = ParentGraphicsState;
                while(parent != null)
                {
                    depth++;
                    parent = parent.ParentGraphicsState;
                }

                return depth;
            }
        }

        public RenderMatrix LocalCTM { get; set; }
        public float? LocalLineWidth { get; set; }
        public int? LocalLineCapStyle { get; set; }
        public int? LocalLineJoinStyle { get; set; }
        public float? LocalMiterLength { get; set; }
        public float[] LocalDashArray { get; set; }
        public int? LocalDashPhase { get; set; }
        public string LocalRenderingIntent { get; set; }
        public float? LocalFlatness { get; set; }
        public bool? LocalOverPrint10 { get; set; }
        public bool? LocalOverPrint13 { get; set; }
        public int? LocalOverPrintMode { get; set; }
        public float? LocalConstantAlphaStroking { get; set; }
        public float? LocalConstantAlphaNonStroking { get; set; }
        public float? LocalFlatnessTolerance { get; set; }
        public float? LocalSmoothnessTolerance { get; set; }
        public bool? LocalStrokeAdjustment { get; set; }
        public bool? LocalAlphaSourceMask { get; set; }
        public bool? LocalTextKnockout { get; set; }

        public RenderMatrix CTM
        {
            get
            {
                if (LocalCTM != null)
                    return LocalCTM;
                else
                    return ParentGraphicsState.CTM;
            }

            set
            {
                if (LocalCTM == null)
                {
                    LocalCTM = CTM;
                    if (LocalCTM == null)
                        LocalCTM = value;
                    else
                    {
                        LocalCTM = LocalCTM.Clone();
                        LocalCTM.Multiply(value);
                    }
                }
                else
                    LocalCTM.Multiply(value);
            }
        }


        public float LineWidth
        {
            get
            {
                if (LocalLineWidth.HasValue)
                    return LocalLineWidth.Value;
                else
                    return ParentGraphicsState.LineWidth;
            }

            set { LocalLineWidth = Math.Max(0, value); }
        }

        public int LineCapStyle
        {
            get
            {
                if (LocalLineCapStyle.HasValue)
                    return LocalLineCapStyle.Value;
                else
                    return ParentGraphicsState.LineCapStyle;
            }

            set { LocalLineCapStyle = Math.Max(0, value); }
        }

        public int LineJoinStyle
        {
            get
            {
                if (LocalLineJoinStyle.HasValue)
                    return LocalLineJoinStyle.Value;
                else
                    return ParentGraphicsState.LineJoinStyle;
            }

            set { LocalLineJoinStyle = Math.Max(0, value); }
        }

        public float MiterLength
        {
            get
            {
                if (LocalMiterLength.HasValue)
                    return LocalMiterLength.Value;
                else
                    return ParentGraphicsState.MiterLength;
            }

            set { LocalMiterLength = value; }
        }

        public float[] DashArray
        {
            get
            {
                if (LocalDashArray != null)
                    return LocalDashArray;
                else
                    return ParentGraphicsState.DashArray;
            }

            set { LocalDashArray = value; }
        }

        public int DashPhase
        {
            get
            {
                if (LocalDashPhase.HasValue)
                    return LocalDashPhase.Value;
                else
                    return ParentGraphicsState.DashPhase;
            }

            set { LocalDashPhase = Math.Max(0, value); }
        }

        public string RenderingIntent
        {
            get
            {
                if (LocalRenderingIntent != null)
                    return LocalRenderingIntent;
                else
                    return ParentGraphicsState.RenderingIntent;
            }

            set { LocalRenderingIntent = value; }
        }

        public float Flatness
        {
            get
            {
                if (LocalFlatness.HasValue)
                    return LocalFlatness.Value;
                else
                    return ParentGraphicsState.Flatness;
            }

            set { LocalFlatness = Math.Min(100, Math.Max(0, value)); }
        }

        public bool? OverPrint10
        {
            get
            {
                if (LocalOverPrint10.HasValue)
                    return LocalOverPrint10.Value;
                else
                    return ParentGraphicsState.OverPrint10;
            }

            set { LocalOverPrint10 = value; }
        }

        public bool? OverPrint13
        {
            get
            {
                if (LocalOverPrint13.HasValue)
                    return LocalOverPrint13.Value;
                else
                    return ParentGraphicsState.OverPrint13;
            }

            set { LocalOverPrint13 = value; }
        }

        public int OverPrintMode
        {
            get
            {
                if (LocalOverPrintMode.HasValue)
                    return LocalOverPrintMode.Value;
                else
                    return ParentGraphicsState.OverPrintMode;
            }

            set { LocalOverPrintMode = value; }
        }

        public float? ConstantAlphaStroking
        {
            get
            {
                if (LocalConstantAlphaStroking.HasValue)
                    return LocalConstantAlphaStroking.Value;
                else
                    return ParentGraphicsState.ConstantAlphaStroking;
            }

            set { LocalConstantAlphaStroking = value; }
        }

        public float? ConstantAlphaNonStroking
        {
            get
            {
                if (LocalConstantAlphaNonStroking.HasValue)
                    return LocalConstantAlphaNonStroking.Value;
                else
                    return ParentGraphicsState.ConstantAlphaNonStroking;
            }

            set { LocalConstantAlphaNonStroking = value; }
        }

        public float? FlatnessTolerance
        {
            get
            {
                if (LocalFlatnessTolerance.HasValue)
                    return LocalFlatnessTolerance.Value;
                else
                    return ParentGraphicsState.FlatnessTolerance;
            }

            set { LocalFlatnessTolerance = value; }
        }

        public float? SmoothnessTolerance
        {
            get
            {
                if (LocalSmoothnessTolerance.HasValue)
                    return LocalSmoothnessTolerance.Value;
                else
                    return ParentGraphicsState.SmoothnessTolerance;
            }

            set { LocalSmoothnessTolerance = value; }
        }

        public bool? StrokeAdjustment
        {
            get
            {
                if (LocalStrokeAdjustment.HasValue)
                    return LocalStrokeAdjustment.Value;
                else
                    return ParentGraphicsState.StrokeAdjustment;
            }

            set { LocalStrokeAdjustment = value; }
        }

        public bool? AlphaSourceMask
        {
            get
            {
                if (LocalAlphaSourceMask.HasValue)
                    return LocalAlphaSourceMask.Value;
                else
                    return ParentGraphicsState.AlphaSourceMask;
            }

            set { LocalAlphaSourceMask = value; }
        }

        public bool? TextKnockout
        {
            get
            {
                if (LocalTextKnockout.HasValue)
                    return LocalTextKnockout.Value;
                else
                    return ParentGraphicsState.TextKnockout;
            }

            set { LocalTextKnockout = value; }
        }

        public RenderColorSpace ColorSpaceStroking
        {
            get
            {
                if (_colorSpaceStroking != null)
                    return _colorSpaceStroking;
                else
                    return ParentGraphicsState.ColorSpaceStroking;
            }

            set { _colorSpaceStroking = value; }
        }

        public RenderColorSpace ColorSpaceNonStroking
        {
            get
            {
                if (_colorSpaceNonStroking != null)
                    return _colorSpaceNonStroking;
                else
                    return ParentGraphicsState.ColorSpaceNonStroking;
            }

            set { _colorSpaceNonStroking = value; }
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

        public object Clipping
        {
            get
            {
                if (_clip != null)
                    return _clip;
                else
                    return ParentGraphicsState.Clipping;
            }

            set { _clip = value; }
        }
    }
}
