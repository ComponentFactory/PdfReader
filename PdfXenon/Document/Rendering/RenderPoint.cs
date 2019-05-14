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

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public float X { get; set; }
        public float Y { get; set; }

        public float Distance(RenderPoint pt)
        {
            if (X == pt.X)
            {
                if (Y == pt.Y)
                {
                    // Identical points have no distance between them
                    return 0;
                }
                else
                {
                    // Save X position, so distance is the Y difference
                    return Math.Abs(Y - pt.Y);
                }
            }
            else if (Y == pt.Y)
            {
                // Save Y position, so distance is the X difference
                return Math.Abs(X - pt.X);
            }
            else
            {
                float xDiff = (Math.Max(X, pt.X) - Math.Min(X, pt.X));
                float yDiff = (Math.Max(Y, pt.Y) - Math.Min(Y, pt.Y));
                return (float)Math.Sqrt((xDiff * xDiff) + (yDiff * yDiff));

            }
        }
    }
}
