using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfMatrix
    {
        public PdfMatrix()
        {
            M11 = 1;
            M22 = 1;
        }

        public PdfMatrix(float m11, float m12, float m21, float m22, float offsetX, float offsetY)
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public override string ToString()
        {
            return $"{M11},{M12} {M21},{M22} {OffsetX},{OffsetY}";
        }

        public float M11 { get; private set; }
        public float M12 { get; private set; }
        public float M21 { get; private set; }
        public float M22 { get; private set; }
        public float OffsetX { get; private set; }
        public float OffsetY { get; private set; }

        public void Translate(float x, float y)
        {
            // Only need to adjust the offsets
            OffsetX += x;
            OffsetY += y;
        }

        public void Scale(float scaleX, float scaleY)
        {
            // Only need to adjust the X, Y values
            Multiply(scaleX, 0, 0, scaleY, 0, 0);
        }

        public void RotateAt(float angle, float centreX, float centreY)
        {
            // We take degrees but need to convert to using radians
            float radians = (float)((angle % 360) * (Math.PI / 180));
            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);
            Multiply(cos, sin, 
                    -sin, cos,  
                    (float)(centreX * (1.0 - cos)) + (centreY * sin), 
                    (float)(centreY * (1.0 - cos)) - (centreX * sin));
        }

        public PdfPoint Transform(float x, float y)
        {
            float tempX = (float)((x * M11) + (y * M21) + OffsetX);
            return new PdfPoint(tempX, (x * M12) + (y * M22) + OffsetY);
        }

        public PdfPoint Transform(PdfPoint pt)
        {
            float tempX = (float)((pt.X * M11) + (pt.Y * M21) + OffsetX);
            pt.Y = (pt.X * M12) + (pt.Y * M22) + OffsetY;
            pt.X = tempX;
            return pt;
        }

        public void Transform(PdfPoint[] pts)
        {
            foreach(PdfPoint pt in pts)
                Transform(pt);
        }

        public void Transform(List<PdfPoint> pts)
        {
            foreach (PdfPoint pt in pts)
                Transform(pt);
        }

        public void Multiply(PdfMatrix matrix)
        {
            Multiply(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
        }

        public PdfMatrix Clone()
        {
            return new PdfMatrix(M11, M12, M21, M22, OffsetX, OffsetY);
        }

        private void Multiply(float m11, float m12, float m21, float m22, float offsetX, float offsetY)
        {
            float tm11 = (M11 * m11) + (M12 * m21);
            float tm12 = (M11 * m12) + (M12 * m22);
            float tm21 = (M21 * m11) + (M22 * m21);
            M22 = (M21 * m12) + (M22 * m22);
            M21 = tm21;
            M12 = tm12;
            M11 = tm11;
            OffsetX = (OffsetX * m11) + (OffsetY * m21) + offsetX;
            OffsetY = (OffsetX * m12) + (OffsetY * m22) + offsetY;
        }
    }
}
