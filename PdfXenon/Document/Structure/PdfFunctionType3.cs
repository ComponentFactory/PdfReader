using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfFunctionType3 : PdfFunction
    {
        private List<PdfFunction> _functions;
        private float[] _boundValues;
        private float[] _encodeValues;

        public PdfFunctionType3(PdfObject parent, PdfDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfArray Functions { get => Dictionary.MandatoryValue<PdfArray>("Functions"); }
        public PdfArray Bounds { get => Dictionary.MandatoryValue<PdfArray>("Bounds"); }
        public PdfArray Encode { get => Dictionary.MandatoryValue<PdfArray>("Encode"); }

        public override float[] Call(float[] inputs)
        {
            if (inputs.Length != 1)
                throw new ArgumentOutOfRangeException($"Provided with '{inputs.Length}' values but Function Type 3 is defined to take 1 value.");

            // Find the function that handles values below the Bounds value
            for (int i = 0, d = 0; i < _boundValues.Length; i++, d += 2)
                if (inputs[0] < _boundValues[i])
                    return _functions[i].Call(new float[] { Interpolate(inputs[0], (i == 0) ? _domainValues[0] : _boundValues[i - 1], 
                                                                        _boundValues[i], 
                                                                        _encodeValues[d], 
                                                                        _encodeValues[d + 1]) });

            // Use the last function
            return _functions[_functions.Count - 1].Call(new float[] { Interpolate(inputs[0], 
                                                                                   _boundValues[_boundValues.Length - 1], 
                                                                                   _domainValues[_domainValues.Length - 1], 
                                                                                   _encodeValues[_encodeValues.Length - 2], 
                                                                                   _encodeValues[_encodeValues.Length - 1]) });
        }

        protected override void Initialize()
        {
            base.Initialize();

            _functions = new List<PdfFunction>();
            foreach (PdfObject obj in Functions.Objects)
                _functions.Add(FromObject(this, obj));

            _boundValues = Bounds.AsNumberArray();
            _encodeValues = Encode.AsNumberArray();
        }
    }
}
