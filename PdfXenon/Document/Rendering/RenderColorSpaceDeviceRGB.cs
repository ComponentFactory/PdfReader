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

        public override void ParseParameters()
        {
            _rgb[2] = Renderer.OperandAsNumber();
            _rgb[1] = Renderer.OperandAsNumber();
            _rgb[0] = Renderer.OperandAsNumber();
        }

        public override RenderColorRGB GetColorRGB()
        {
            return new RenderColorRGB(_rgb[0], _rgb[1], _rgb[2]);
        }
    }
}
