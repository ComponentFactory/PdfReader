using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfCalRGB : PdfColorSpace
    {
        private float[] _abc = new float[] { 0f, 0f, 0f };
        private float[] _whitePoint = new float[] { 0f, 1f, 0f };
        private float[] _blackPoint = new float[] { 0f, 0f, 0f };
        private float[] _gamma = new float[] { 1f, 1f, 1f };
        private float[] _matrix = new float[] { 1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f };

        public PdfCalRGB(PdfRenderer renderer, PdfDictionary dictionary)
            : base(renderer)
        {
            PdfArray array = dictionary.MandatoryValue<PdfArray>("WhitePoint");
            for (int i = 0; i < _whitePoint.Length; i++)
                _whitePoint[i] = AsNumber(array.Objects[i]);

            array = dictionary.OptionalValue<PdfArray>("BlackPoint");
            if (array != null)
            {
                for (int i = 0; i < _blackPoint.Length; i++)
                    _blackPoint[i] = AsNumber(array.Objects[i]);
            }

            array = dictionary.OptionalValue<PdfArray>("Gamma");
            if (array != null)
            {
                for (int i = 0; i < _gamma.Length; i++)
                    _gamma[i] = AsNumber(array.Objects[i]);
            }

            array = dictionary.OptionalValue<PdfArray>("Matrix");
            if (array != null)
            {
                for(int i=0; i<_matrix.Length; i++)
                    _matrix[i] = AsNumber(array.Objects[i]);
            }
        }

        public override void ParseColor()
        {
            _abc[2] = Renderer.OperandAsNumber();
            _abc[1] = Renderer.OperandAsNumber();
            _abc[0] = Renderer.OperandAsNumber();
        }

        public override PdfRGB ColorAsRGB()
        {
            float r = (float)((_matrix[0] * Math.Pow(_abc[0], _gamma[0])) + (_matrix[3] * Math.Pow(_abc[1], _gamma[1])) + (_matrix[6] * Math.Pow(_abc[2], _gamma[2])));
            float g = (float)((_matrix[1] * Math.Pow(_abc[0], _gamma[0])) + (_matrix[4] * Math.Pow(_abc[1], _gamma[1])) + (_matrix[7] * Math.Pow(_abc[2], _gamma[2])));
            float b = (float)((_matrix[2] * Math.Pow(_abc[0], _gamma[0])) + (_matrix[5] * Math.Pow(_abc[1], _gamma[1])) + (_matrix[8] * Math.Pow(_abc[2], _gamma[2])));

            return new PdfRGB(Math.Max(0f, Math.Min(1, r)),
                              Math.Max(0f, Math.Min(1, g)),
                              Math.Max(0f, Math.Min(1, b)));
        }

        private float AsNumber(PdfObject obj)
        {
            if (obj is PdfReal real)
                return real.Value;
            else if (obj is PdfInteger integer)
                return integer.Value;

            throw new ApplicationException($"Object of type '{obj.GetType().Name}' found instead of a number.");
        }
    }
}
