using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorRGB
    {
        public PdfColorRGB(float r, float g, float b)
        {
            R = Math.Max(0f, Math.Min(1, r));
            G = Math.Max(0f, Math.Min(1, g));
            B = Math.Max(0f, Math.Min(1, b));
        }

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
    }
}
