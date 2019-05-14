using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfFunctionType2 : PdfFunction
    {
        private float _n;
        private float[] _c0;
        private float[] _c1;

        public PdfFunctionType2(PdfObject parent, PdfDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfInteger N { get => Dictionary.MandatoryValue<PdfInteger>("N"); }
        public PdfArray C0 { get => Dictionary.OptionalValue<PdfArray>("C0"); }
        public PdfArray C1 { get => Dictionary.OptionalValue<PdfArray>("C1"); }

        public override float[] Call(float[] inputs)
        {
            if (inputs.Length != 1)
                throw new ArgumentOutOfRangeException($"Provided with '{inputs.Length}' values but Function Type 2 is defined to take 1 value.");

            float input = inputs[0];
            float[] outputs = new float[_c0.Length];

            // Exponential interpolation between the c0 and c1 values
            for(int i=0; i<outputs.Length; i++)
                outputs[i] = _c0[i] + (float)Math.Pow(input, _n) * (_c1[i] - _c0[i]);

            return outputs;
        }

        protected override void Initialize()
        {
            base.Initialize();

            // Extract and cache values from the dictionary
            _n = N.Value;

            if (C0 != null)
                _c0 = C0.AsNumberArray();
            else
                _c0 = new float[] { 0f };

            if (C1 != null)
                _c1 = C1.AsNumberArray();
            else
                _c1 = new float[] { 1f };
        }
    }
}
