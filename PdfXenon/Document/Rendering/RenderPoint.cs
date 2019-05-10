using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderPoint
    {
        public RenderPoint()
            : this(0f, 0f)
        {
        }

        public RenderPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; set; }
        public float Y { get; set; }
    }
}
