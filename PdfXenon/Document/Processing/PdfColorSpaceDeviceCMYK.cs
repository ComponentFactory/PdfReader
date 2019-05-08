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
            return new PdfColorRGB(1, 0, 0);
        }
    }
}
