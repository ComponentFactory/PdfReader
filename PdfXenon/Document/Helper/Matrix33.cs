using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfXenon.Standard
{
    public class Matrix33
    {
        private float[] _m = new float[9];

        public Matrix33(float[] m)
        {
            for (int i = 0; i < _m.Length; i++)
                _m[i] = m[i];
        }

        public Matrix33(float a, float b, float c,
                        float d, float e, float f,
                        float g, float h, float i)
        {
            _m[0] = a;
            _m[1] = b;
            _m[2] = c;
            _m[3] = d;
            _m[4] = e;
            _m[5] = f;
            _m[6] = g;
            _m[7] = h;
            _m[8] = i;
        }

        public float this[int index]
        {
            get { return _m[index]; }
            set { _m[index] = value; }
        }

        public Matrix33 Inverse()
        {
            float determinant = _m[0] * (_m[4] * _m[8] - _m[5] * _m[7]) - 
                                _m[1] * (_m[8] * _m[3] - _m[5] * _m[6]) + 
                                _m[2] * (_m[3] * _m[7] - _m[4] * _m[6]); ;

            return new Matrix33( (_m[4]*_m[8] - _m[5]*_m[7]) / determinant,
                                -(_m[1]*_m[8] - _m[2]*_m[7]) / determinant,
                                 (_m[1]*_m[5] - _m[2]*_m[4]) / determinant,
                                -(_m[3]*_m[8] - _m[5]*_m[6]) / determinant,
                                 (_m[0]*_m[8] - _m[2]*_m[6]) / determinant,
                                -(_m[0]*_m[5] - _m[2]*_m[3]) / determinant,
                                 (_m[3]*_m[7] - _m[4]*_m[6]) / determinant,
                                -(_m[0]*_m[7] - _m[1]*_m[6]) / determinant,
                                 (_m[0]*_m[4] - _m[1]*_m[3]) / determinant);
        }

        public Matrix33 Multiply(Matrix33 m)
        {
            return new Matrix33(_m[0] * m[0] + _m[1] * m[3] + _m[2] * m[6],
                                _m[0] * m[1] + _m[1] * m[4] + _m[2] * m[7],
                                _m[0] * m[2] + _m[1] * m[5] + _m[2] * m[8],
                                _m[3] * m[0] + _m[4] * m[3] + _m[5] * m[6],
                                _m[3] * m[1] + _m[4] * m[4] + _m[5] * m[7],
                                _m[3] * m[2] + _m[4] * m[5] + _m[5] * m[8],
                                _m[6] * m[0] + _m[7] * m[3] + _m[8] * m[6],
                                _m[6] * m[1] + _m[7] * m[4] + _m[8] * m[7],
                                _m[6] * m[2] + _m[7] * m[5] + _m[8] * m[8]);
        }

        public Matrix31 Transform(Matrix31 m)
        {
            return new Matrix31(_m[0] * m[0] + _m[1] * m[1] + _m[2] * m[2],
                                _m[3] * m[0] + _m[4] * m[1] + _m[5] * m[2],
                                _m[6] * m[0] + _m[7] * m[1] + _m[8] * m[2]);
        }
    }
}
