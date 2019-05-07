using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfMatrix
    {
        private float _m11;
        private float _m12;
        private float _m21;
        private float _m22;
        private float _offsetX;
        private float _offsetY;

        public PdfMatrix()
        {
            // Identity matrix
            _m11 = 1;
            _m22 = 1;
        }

        public void Translate(float x, float y)
        {
            // Only need to adjust the offsets
            _offsetX += x;
            _offsetY += y;
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

        public void Transform(PdfPoint pt)
        {
            float tempX = (float)((pt.X * _m11) + (pt.Y * _m21) + _offsetX);
            pt.Y = (pt.X * _m12) + (pt.Y * _m22) + _offsetY;
            pt.X = tempX;
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

        private void Multiply(float m11, float m12, float m21, float m22, float offsetX, float offsetY)
        {
            float tm11 = (_m11 * m11) + (_m12 * m21);
            float tm12 = (_m11 * m12) + (_m12 * m22);
            float tm21 = (_m21 * m11) + (_m22 * m21);
            _m22 = (_m21 * m12) + (_m22 * m22);
            _m21 = tm21;
            _m12 = tm12;
            _m11 = tm11;
            _offsetX = (_offsetX * m11) + (_offsetY * m21) + offsetX;
            _offsetY = (_offsetX * m12) + (_offsetY * m22) + offsetY;
        }
    }
}
