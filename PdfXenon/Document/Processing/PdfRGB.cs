using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfRGB
    {
        public PdfRGB(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
    }
}
