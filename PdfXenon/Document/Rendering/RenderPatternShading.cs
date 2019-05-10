using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderPatternShading : RenderPatternType
    {
        public RenderPatternShading(RenderObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            Dictionary = dictionary;
        }

        public PdfDictionary Dictionary { get; private set; }

        public PdfInteger ShadingType { get => Dictionary.MandatoryValue<PdfInteger>("ShadingType"); }
        public PdfObject ColorSpace { get => Dictionary.MandatoryValue<PdfObject>("ColorSpace"); }
        public PdfArray Background { get => Dictionary.OptionalValue<PdfArray>("Background"); }
        public PdfRectangle BBox { get => PdfObject.ArrayToRectangle(Dictionary.OptionalValue<PdfArray>("BBox")); }
        public PdfBoolean AntiAlias { get => Dictionary.OptionalValue<PdfBoolean>("AntiAlias"); }
    }
}
