using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorSpaceCalGray : PdfColorSpace
    {
        private float _a = 0f;
        private float[] _whitePoint = new float[] { 0f, 1f, 0f };
        private float[] _blackPoint = new float[] { 0f, 0f, 0f };
        private float _gamma = 1f;

        public PdfColorSpaceCalGray(PdfRenderer renderer, PdfDictionary dictionary)
            : base(renderer)
        {
            PdfArray array = dictionary.MandatoryValue<PdfArray>("WhitePoint");
            for (int i = 0; i < _whitePoint.Length; i++)
                _whitePoint[i] = Renderer.AsNumber(array.Objects[i]);

            array = dictionary.OptionalValue<PdfArray>("BlackPoint");
            if (array != null)
            {
                for (int i = 0; i < _blackPoint.Length; i++)
                    _blackPoint[i] = Renderer.AsNumber(array.Objects[i]);
            }

            PdfObject obj = dictionary.OptionalValue<PdfObject>("Gamma");
            if (obj != null)
                _gamma = Renderer.AsNumber(obj);
        }

        public override void ParseParameters()
        {
            _a = Renderer.OperandAsNumber();
        }

        public override bool IsColor { get => true; }

        public override PdfColorRGB GetColor()
        {
            return new PdfColorRGB((float)(_whitePoint[0] * Math.Pow(_a, _gamma)),
                                   (float)(_whitePoint[1] * Math.Pow(_a, _gamma)),
                                   (float)(_whitePoint[2] * Math.Pow(_a, _gamma)));
        }
    }
}
