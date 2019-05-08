using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDeviceRGB : PdfColorSpace
    {
        private float[] _rgb = new float[] { 0f, 0f, 0f };

        public PdfDeviceRGB(PdfRenderer renderer)
            : base(renderer)
        {
        }

        public override void ParseColor()
        {
            _rgb[2] = Renderer.OperandAsNumber();
            _rgb[1] = Renderer.OperandAsNumber();
            _rgb[0] = Renderer.OperandAsNumber();
        }

        public override PdfRGB ColorAsRGB()
        {
            return new PdfRGB(_rgb[0], _rgb[1], _rgb[2]);
        }
    }
}
