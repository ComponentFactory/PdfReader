using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderPatternShading : RenderPatternType
    {
        public RenderPatternShading(RenderObject parent, PdfDictionary extGState, PdfArray matrix, PdfDictionary dictionary)
            : base(parent)
        {
            ExtGState = extGState;
            Matrix = matrix;
            Dictionary = dictionary;
        }

        public PdfDictionary ExtGState { get; private set; }
        public PdfArray Matrix { get; private set; }
        public PdfDictionary Dictionary { get; private set; }

        public PdfInteger ShadingType { get => Dictionary.MandatoryValue<PdfInteger>("ShadingType"); }
        public PdfName ColorSpace { get => Dictionary.MandatoryValue<PdfName>("ColorSpace"); }
        public PdfArray Background { get => Dictionary.OptionalValue<PdfArray>("Background"); }
        public PdfRectangle BBox { get => PdfObject.ArrayToRectangle(Dictionary.OptionalValue<PdfArray>("BBox")); }
        public PdfBoolean AntiAlias { get => Dictionary.OptionalValue<PdfBoolean>("AntiAlias"); }

        public RenderColorSpace ColorSpaceValue { get { return RenderColorSpace.FromName(Parent, ColorSpace.Value); } }
    }
}
