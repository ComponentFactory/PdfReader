using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderColorSpaceDeviceGray : RenderColorSpaceRGB
    {
        private float _gray = 0f;

        public RenderColorSpaceDeviceGray(RenderObject parent)
            : base(parent)
        {
        }

        public override int NumberOfComponents()
        {
            return 1;
        }

        public override void Parse(float[] values)
        {
            _gray = values[0];
        }

        public override void ParseParameters()
        {
            Parse(new float[] { Renderer.OperandAsNumber() });
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB(_gray, _gray, _gray);
        }
    }
}
