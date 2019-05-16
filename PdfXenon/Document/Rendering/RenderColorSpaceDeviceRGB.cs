using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderColorSpaceDeviceRGB : RenderColorSpaceRGB
    {
        private float[] _rgb = new float[] { 0f, 0f, 0f };

        public RenderColorSpaceDeviceRGB(RenderObject parent)
            : base(parent)
        {
        }

        public override int NumberOfComponents()
        {
            return 3;
        }

        public override void Parse(float[] values)
        {
            _rgb = values;
        }

        public override void ParseParameters()
        {
            float[] values = new float[3];
            values[2] = Renderer.OperandAsNumber();
            values[1] = Renderer.OperandAsNumber();
            values[0] = Renderer.OperandAsNumber();
            Parse(values);
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB(_rgb[0], _rgb[1], _rgb[2]);
        }
    }
}
