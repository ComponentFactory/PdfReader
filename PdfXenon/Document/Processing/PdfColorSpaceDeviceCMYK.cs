using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorSpaceDeviceCMYK : PdfColorSpace
    {
        private float[] _cmyk = new float[] { 0f, 0f, 0f, 1f };

        public PdfColorSpaceDeviceCMYK(PdfRenderer renderer)
            : base(renderer)
        {
        }

        public override void ParseColor()
        {
            _cmyk[3] = Renderer.OperandAsNumber();
            _cmyk[2] = Renderer.OperandAsNumber();
            _cmyk[1] = Renderer.OperandAsNumber();
            _cmyk[0] = Renderer.OperandAsNumber();
        }

        public override PdfColorRGB ColorAsRGB()
        {
            float r = (1 - _cmyk[0]) * (1 - _cmyk[3]);
            float g = (1 - _cmyk[1]) * (1 - _cmyk[3]);
            float b = (1 - _cmyk[2]) * (1 - _cmyk[3]);

            return new PdfColorRGB(Math.Max(0f, Math.Min(1, r)),
                                   Math.Max(0f, Math.Min(1, g)),
                                   Math.Max(0f, Math.Min(1, b)));
        }
    }
}
