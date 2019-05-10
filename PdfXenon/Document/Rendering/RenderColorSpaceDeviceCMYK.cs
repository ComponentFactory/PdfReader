using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderColorSpaceDeviceCMYK : RenderColorSpaceRGB
    {
        private float[] _cmyk = new float[] { 0f, 0f, 0f, 1f };

        public RenderColorSpaceDeviceCMYK(RenderObject parent)
            : base(parent)
        {
        }

        public override void ParseParameters()
        {
            _cmyk[3] = Renderer.OperandAsNumber();
            _cmyk[2] = Renderer.OperandAsNumber();
            _cmyk[1] = Renderer.OperandAsNumber();
            _cmyk[0] = Renderer.OperandAsNumber();
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB((1 - _cmyk[0]) * (1 - _cmyk[3]),
                                      (1 - _cmyk[1]) * (1 - _cmyk[3]),
                                      (1 - _cmyk[2]) * (1 - _cmyk[3]));
        }
    }
}
