using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorSpacePattern : PdfColorSpace
    {
        private string _name;
        private PdfColorSpace _colorSpace;

        public PdfColorSpacePattern(PdfRenderer renderer)
            : this(renderer, null)
        {
        }

        public PdfColorSpacePattern(PdfRenderer renderer, PdfColorSpace colorSpace)
            : base(renderer)
        {
            _colorSpace = colorSpace;
        }

        public override void ParseColor()
        {
            _name = Renderer.OperandAsString();
        }

        public override PdfColorRGB ColorAsRGB()
        {
            return new PdfColorRGB(1, 0, 0);
        }
    }
}
