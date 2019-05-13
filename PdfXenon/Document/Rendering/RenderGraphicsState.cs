using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderGraphicsState : RenderObject
    {
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
        public RenderColorSpace LocalColorSpaceStroking { get; set; }
        public RenderColorSpace LocalColorSpaceNonStroking { get; set; }
        public PdfObject LocalBlendMode { get; set; }
        public PdfObject LocalFont { get; set; }
        public PdfObject LocalBlackGeneration { get; set; }
        public PdfObject LocalBlackGeneration2 { get; set; }
        public PdfObject LocalUndercolorRemoval { get; set; }
        public PdfObject LocalUndercolorRemoval2 { get; set; }
        public PdfObject LocalTransfer { get; set; }
        public PdfObject LocalTransfer2 { get; set; }
        public PdfObject LocalHalftone { get; set; }
        public PdfObject LocalSoftMask { get; set; }
        public object LocalClipping { get; set; }

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
                if (LocalColorSpaceStroking != null)
                    return LocalColorSpaceStroking;
                else
                    return ParentGraphicsState.ColorSpaceStroking;
            }

            set { LocalColorSpaceStroking = value; }
        }

        public RenderColorSpace ColorSpaceNonStroking
        {
            get
            {
                if (LocalColorSpaceNonStroking != null)
                    return LocalColorSpaceNonStroking;
                else
                    return ParentGraphicsState.ColorSpaceNonStroking;
            }

            set { LocalColorSpaceNonStroking = value; }
        }

        public PdfObject BlendMode
        {
            get
            {
                if (LocalBlendMode != null)
                    return LocalBlendMode;
                else
                    return ParentGraphicsState.BlendMode;
            }

            set { LocalBlendMode = value; }
        }

        public PdfObject Font
        {
            get
            {
                if (LocalFont != null)
                    return LocalFont;
                else
                    return ParentGraphicsState.Font;
            }

            set { LocalFont = value; }
        }

        public PdfObject BlackGeneration
        {
            get
            {
                if (LocalBlackGeneration != null)
                    return LocalBlackGeneration;
                else
                    return ParentGraphicsState.BlackGeneration;
            }

            set { LocalBlackGeneration = value; }
        }

        public PdfObject BlackGeneration2
        {
            get
            {
                if (LocalBlackGeneration2 != null)
                    return LocalBlackGeneration2;
                else
                    return ParentGraphicsState.BlackGeneration2;
            }

            set { LocalBlackGeneration2 = value; }
        }

        public PdfObject UndercolorRemoval
        {
            get
            {
                if (LocalUndercolorRemoval != null)
                    return LocalUndercolorRemoval;
                else
                    return ParentGraphicsState.UndercolorRemoval;
            }

            set { LocalUndercolorRemoval = value; }
        }

        public PdfObject UndercolorRemoval2
        {
            get
            {
                if (LocalUndercolorRemoval2 != null)
                    return LocalUndercolorRemoval2;
                else
                    return ParentGraphicsState.UndercolorRemoval2;
            }

            set { LocalUndercolorRemoval2 = value; }
        }

        public PdfObject Transfer
        {
            get
            {
                if (LocalTransfer != null)
                    return LocalTransfer;
                else
                    return ParentGraphicsState.Transfer;
            }

            set { LocalTransfer = value; }
        }

        public PdfObject Transfer2
        {
            get
            {
                if (LocalTransfer2 != null)
                    return LocalTransfer2;
                else
                    return ParentGraphicsState.Transfer2;
            }

            set { LocalTransfer2 = value; }
        }

        public PdfObject Halftone
        {
            get
            {
                if (LocalHalftone != null)
                    return LocalHalftone;
                else
                    return ParentGraphicsState.Halftone;
            }

            set { LocalHalftone = value; }
        }

        public PdfObject SoftMask
        {
            get
            {
                if (LocalSoftMask != null)
                    return LocalSoftMask;
                else
                    return ParentGraphicsState.SoftMask;
            }

            set { LocalSoftMask = value; }
        }

        public object Clipping
        {
            get
            {
                if (LocalClipping != null)
                    return LocalClipping;
                else
                    return ParentGraphicsState.Clipping;
            }

            set { LocalClipping = value; }
        }
    }
}
