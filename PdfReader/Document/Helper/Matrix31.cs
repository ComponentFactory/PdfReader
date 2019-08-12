using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfReader
{
    public class Matrix31
    {
        private float[] _m = new float[3];

        public Matrix31(float[] m)
        {
            for (int i = 0; i < _m.Length; i++)
                _m[i] = m[i];
        }

        public Matrix31(float a, float b, float c)
        {
            _m[0] = a;
            _m[1] = b;
            _m[2] = c;
        }

        public float this[int index]
        {
            get { return _m[index]; }
            set { _m[index] = value; }
        }
    }
}
