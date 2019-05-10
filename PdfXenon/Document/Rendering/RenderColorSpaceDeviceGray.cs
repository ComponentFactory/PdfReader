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

        public override void ParseParameters()
        {
            _gray = Renderer.OperandAsNumber();
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB(_gray, _gray, _gray);
        }
    }
}
