using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorSpaceDeviceGray : PdfColorSpace
    {
        private float _gray = 0f;

        public PdfColorSpaceDeviceGray(PdfRenderer renderer)
            : base(renderer)
        {
        }

        public override void ParseParameters()
        {
            _gray = Renderer.OperandAsNumber();
        }

        public override bool IsColor { get => true; }

        public override PdfColorRGB GetColor()
        {
            return new PdfColorRGB(_gray, _gray, _gray);
        }
    }
}
