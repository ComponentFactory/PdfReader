using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfPatternShading : PdfPatternType
    {
        public PdfPatternShading(PdfObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            Dictionary = dictionary;
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);

            if (ShadingType != null) sb.Append($"ShadingType: {ShadingType.ToDebug()}\n{blank}");
            if (ColorSpace != null) sb.Append($"ColorSpace: {ColorSpace.ToDebug()}\n{blank}");
            if (Background != null) sb.Append($"Background: {Background.ToDebug()}\n{blank}");
            if (BBox != null) sb.Append($"BBox: {BBox.ToDebug()}\n{blank}");
            if (AntiAlias != null) sb.Append($"AntiAlias: {AntiAlias.ToDebug()}\n{blank}");

            return indent;
        }

        public PdfDictionary Dictionary { get; private set; }

        public PdfInteger ShadingType { get => Dictionary.MandatoryValue<PdfInteger>("ShadingType"); }
        public PdfObject ColorSpace { get => Dictionary.MandatoryValue<PdfObject>("ColorSpace"); }
        public PdfArray Background { get => Dictionary.OptionalValue<PdfArray>("Background"); }
        public PdfRectangle BBox { get => ArrayToRectangle(Dictionary.OptionalValue<PdfArray>("BBox")); }
        public PdfBoolean AntiAlias { get => Dictionary.OptionalValue<PdfBoolean>("AntiAlias"); }
    }
}
