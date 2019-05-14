using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderPatternShading : RenderPatternType
    {
        public RenderPatternShading(RenderObject parent, PdfDictionary dictionary, PdfDictionary extGState, PdfArray matrix)
            : base(parent)
        {
            Dictionary = dictionary;
            ExtGState = extGState;
            Matrix = matrix;
        }

        public PdfDictionary ExtGState { get; set; }
        public PdfArray Matrix { get; set; }
        public PdfDictionary Dictionary { get; set; }

        public PdfInteger ShadingType { get => Dictionary.MandatoryValue<PdfInteger>("ShadingType"); }
        public PdfName ColorSpace { get => Dictionary.MandatoryValue<PdfName>("ColorSpace"); }
        public PdfArray Background { get => Dictionary.OptionalValue<PdfArray>("Background"); }
        public PdfRectangle BBox { get => PdfObject.ArrayToRectangle(Dictionary.OptionalValue<PdfArray>("BBox")); }
        public PdfBoolean AntiAlias { get => Dictionary.OptionalValue<PdfBoolean>("AntiAlias"); }

        public RenderColorSpace ColorSpaceValue { get { return RenderColorSpace.FromName(Parent, ColorSpace.Value); } }

        public static RenderPatternShading ParseShading(RenderObject parent, PdfDictionary shading, PdfDictionary extGState, PdfArray matrix)
        {
            PdfInteger shadingType = shading.MandatoryValue<PdfInteger>("ShadingType");

            switch (shadingType.Value)
            {
                case 2: // Axial Shading
                    return new RenderPatternShadingAxial(parent, shading, extGState, matrix);
                case 3: // Radial Shading
                    return new RenderPatternShadingRadial(parent, shading, extGState, matrix);
                default:
                    throw new NotImplementedException($"Pattern shading type '{shadingType.Value}' not implemented.");
            }
        }
    }
}
