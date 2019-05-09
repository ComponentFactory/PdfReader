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

        public override void ParseParameters()
        {
            _cmyk[3] = Renderer.OperandAsNumber();
            _cmyk[2] = Renderer.OperandAsNumber();
            _cmyk[1] = Renderer.OperandAsNumber();
            _cmyk[0] = Renderer.OperandAsNumber();
        }

        public override bool IsColor { get => true; }

        public override PdfColorRGB GetColor()
        {
            return new PdfColorRGB((1 - _cmyk[0]) * (1 - _cmyk[3]),
                                   (1 - _cmyk[1]) * (1 - _cmyk[3]),
                                   (1 - _cmyk[2]) * (1 - _cmyk[3]));
        }
    }
}
