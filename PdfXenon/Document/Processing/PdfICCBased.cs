using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfICCBased : PdfColorSpace
    {
        public PdfICCBased(PdfRenderer renderer, PdfStream stream)
            : base(renderer)
        {

        }

        public override void ParseColor()
        {
        }

        public override PdfRGB ColorAsRGB()
        {
            throw new NotImplementedException("PdfDeviceCMYK color conversion to RGB.");
        }
    }
}
