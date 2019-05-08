using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfCalGray : PdfColorSpace
    {
        private float _a = 0f;
        private float[] _whitePoint = new float[] { 0f, 1f, 0f };
        private float[] _blackPoint = new float[] { 0f, 0f, 0f };
        private float _gamma = 1f;

        public PdfCalGray(PdfRenderer renderer, PdfDictionary dictionary)
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

            PdfObject obj = dictionary.OptionalValue<PdfObject>("Gamma");
            if (obj != null)
                _gamma = AsNumber(obj);
        }

        public override void ParseColor()
        {
            _a = Renderer.OperandAsNumber();
        }

        public override PdfRGB ColorAsRGB()
        {
            float r = (float)(_whitePoint[0] * Math.Pow(_a, _gamma));
            float g = (float)(_whitePoint[1] * Math.Pow(_a, _gamma));
            float b = (float)(_whitePoint[2] * Math.Pow(_a, _gamma));

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
