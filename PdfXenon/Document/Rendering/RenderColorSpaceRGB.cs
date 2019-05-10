using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderColorSpaceRGB : RenderColorSpace
    {
        public RenderColorSpaceRGB(RenderObject parent)
            : base(parent)
        {
        }

        public abstract RenderColorRGB GetColorRGB();
    }
}
