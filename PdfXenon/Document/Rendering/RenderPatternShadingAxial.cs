using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderPatternShadingAxial : RenderPatternShading
    {
        private PdfFunction _function;

        public RenderPatternShadingAxial(RenderObject parent, PdfDictionary dictionary, PdfDictionary extGState, PdfArray matrix)
            : base(parent, dictionary, extGState, matrix)
        {
        }

        public override void Visit(IRenderObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public PdfArray Coords { get => Dictionary.MandatoryValue<PdfArray>("Coords"); }
        public PdfArray Domain { get => Dictionary.OptionalValue<PdfArray>("Domain"); }
        public PdfObject Function { get => Dictionary.MandatoryValueRef<PdfObject>("Function"); }
        public PdfArray Extend { get => Dictionary.OptionalValue<PdfArray>("Extend"); }

        public PdfFunction FunctionValue
        {
            get
            {
                if (_function == null)
                    _function = PdfFunction.FromObject(Dictionary, Function);

                return _function;
            }
        }
    }
}
