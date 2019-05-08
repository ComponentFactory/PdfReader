using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDeviceGray : PdfColorSpace
    {
        private float _gray = 0f;

        public PdfDeviceGray(PdfRenderer renderer)
            : base(renderer)
        {
        }

        public override void ParseColor()
        {
            _gray = Renderer.OperandAsNumber();
        }

        public override PdfRGB ColorAsRGB()
        {
            return new PdfRGB(_gray, _gray, _gray);
        }
    }
}
