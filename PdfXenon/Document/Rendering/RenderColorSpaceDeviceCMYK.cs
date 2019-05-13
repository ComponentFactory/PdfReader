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

        public override void Parse(float[] values)
        {
            _cmyk = values;
        }

        public override void ParseParameters()
        {
            float[] values = new float[4];
            values[3] = Renderer.OperandAsNumber();
            values[2] = Renderer.OperandAsNumber();
            values[1] = Renderer.OperandAsNumber();
            values[0] = Renderer.OperandAsNumber();
            Parse(values);
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB((1 - _cmyk[0]) * (1 - _cmyk[3]),
                                      (1 - _cmyk[1]) * (1 - _cmyk[3]),
                                      (1 - _cmyk[2]) * (1 - _cmyk[3]));
        }
    }
}
