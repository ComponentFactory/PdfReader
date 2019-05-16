using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderColorSpaceCalGray : RenderColorSpaceRGB
    {
        private float _a = 0f;
        private float[] _whitePoint = new float[] { 0f, 1f, 0f };
        private float[] _blackPoint = new float[] { 0f, 0f, 0f };
        private float _gamma = 1f;

        public RenderColorSpaceCalGray(RenderObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            _whitePoint = dictionary.MandatoryValue<PdfArray>("WhitePoint").AsNumberArray();
            _blackPoint = dictionary.MandatoryValue<PdfArray>("BlackPoint").AsNumberArray();
            _gamma = dictionary.OptionalValue<PdfObject>("Gamma").AsNumber();
        }

        public override int NumberOfComponents()
        {
            return 1;
        }

        public override void Parse(float[] values)
        {
            _a = values[0];
        }

        public override void ParseParameters()
        {
            Parse(new float[] { Renderer.OperandAsNumber() });
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB((float)(_whitePoint[0] * Math.Pow(_a, _gamma)),
                                      (float)(_whitePoint[1] * Math.Pow(_a, _gamma)),
                                      (float)(_whitePoint[2] * Math.Pow(_a, _gamma)));
        }
    }
}
