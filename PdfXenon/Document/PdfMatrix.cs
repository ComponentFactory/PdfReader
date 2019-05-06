using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfMatrix
    {
        private double _m11;
        private double _m12;
        private double _m21;
        private double _m22;
        private double _offsetX;
        private double _offsetY;

        public PdfMatrix()
        {
            // Identity matrix
            _m11 = 1;
            _m22 = 1;
        }

        public void Translate(double x, double y)
        {
            // Only need to adjust the offsets
            _offsetX += x;
            _offsetY += y;
        }

        public void Scale(double scaleX, double scaleY)
        {
            // Only need to adjust the X, Y values
            Multiply(scaleX, 0, 0, scaleY, 0, 0);
        }

        public void RotateAt(double angle, double centreX, double centreY)
        {
            // We take degrees but need to convert to using radians
            double radians = (angle % 360) * (Math.PI / 180);
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);
            Multiply(cos, sin, -sin, cos,  (centreX * (1.0 - cos)) + (centreY * sin), (centreY * (1.0 - cos)) - (centreX * sin));
        }

        public void Transform(PdfPoint pt)
        {
            double tempX = (pt.X * _m11) + (pt.Y * _m21) + _offsetX;
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

        private void Multiply(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
        {
            double tm11 = (_m11 * m11) + (_m12 * m21);
            double tm12 = (_m11 * m12) + (_m12 * m22);
            double tm21 = (_m21 * m11) + (_m22 * m21);
            _m22 = (_m21 * m12) + (_m22 * m22);
            _m21 = tm21;
            _m12 = tm12;
            _m11 = tm11;
            _offsetX = (_offsetX * m11) + (_offsetY * m21) + offsetX;
            _offsetY = (_offsetX * m12) + (_offsetY * m22) + offsetY;
        }
    }
}
