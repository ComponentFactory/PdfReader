using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorSpaceDeviceRGB : PdfColorSpace
    {
        private float[] _rgb = new float[] { 0f, 0f, 0f };

        public PdfColorSpaceDeviceRGB(PdfRenderer renderer)
            : base(renderer)
        {
        }

        public override void ParseParameters()
        {
            _rgb[2] = Renderer.OperandAsNumber();
            _rgb[1] = Renderer.OperandAsNumber();
            _rgb[0] = Renderer.OperandAsNumber();
        }

        public override bool IsColor { get => true; }

        public override PdfColorRGB GetColor()
        {
            return new PdfColorRGB(_rgb[0], _rgb[1], _rgb[2]);
        }
    }
}
