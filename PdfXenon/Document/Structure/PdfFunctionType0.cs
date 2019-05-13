using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfFunctionType0 : PdfFunction
    {
        private float[] _sizeValues;
        private int _bitsPerSampleValue;
        private int _orderValue;
        private float[] _encodeValues;
        private float[] _decodeValues;
        private int[] _samplesValues;

        public PdfFunctionType0(PdfObject parent, PdfStream stream)
            : base(parent, stream.Dictionary)
        {
            Stream = stream;
        }

        public PdfStream Stream { get; private set; }
        public PdfArray Size { get => Dictionary.MandatoryValue<PdfArray>("Size"); }
        public PdfInteger BitsPerSample { get => Dictionary.MandatoryValue<PdfInteger>("BitsPerSample"); }
        public PdfInteger Order { get => Dictionary.OptionalValue<PdfInteger>("Order"); }
        public PdfArray Encode { get => Dictionary.OptionalValue<PdfArray>("Encode"); }
        public PdfArray Decode { get => Dictionary.OptionalValue<PdfArray>("Decode"); }

        public override float[] Call(float[] inputs)
        {
            if (inputs.Length != _sizeValues.Length)
                throw new ArgumentOutOfRangeException($"Provided with '{inputs.Length}' values but Function Type 0 is defined to take '{_sizeValues.Length}' values.");

            int sampleNumber = 0;
            for(int i=0, d=0; i<inputs.Length; i++, d += 2)
            {
                // Limit check the input to the domain range
                inputs[i] = Math.Max(_domainValues[d], Math.Min(_domainValues[d + 1], inputs[i]));

                // Interpolate each input from the domain to the set of encoded values
                inputs[i] = (int)Interpolate(inputs[i], _domainValues[d], _domainValues[d + 1], _encodeValues[d], _encodeValues[d + 1]);

                // Limit check to the encoded values
                inputs[i] = Math.Max(_encodeValues[d], Math.Min(_encodeValues[d + 1], inputs[i]));

                // Find sample position within array
                sampleNumber += (int)(inputs[i] * _samplesValues[i]);
            }

            int numOutputs = _rangeValues.Length / 2;

            // Find the offset in bits to the first output sample value
            int bitsOffset = sampleNumber * _bitsPerSampleValue * numOutputs;

            float[] outputs = new float[numOutputs];
            for(int i=0, d = 0; i< numOutputs; i++, d += 2)
            {
                int sampleValue = 0;
                int sampleValueMax = 0;
                switch (_bitsPerSampleValue)
                {
                    case 8:
                        // Find the byte that contains the output
                        sampleValue = Stream.ValueAsBytes[bitsOffset / 8];
                        sampleValueMax = 255;
                        break;
                    default:
                        throw new NotImplementedException($"Function Type 0  with BitsPerSample of '{BitsPerSample}' not implemented.");
                }

                // Interpolate each output from te sample to the decode values
                outputs[i] = Interpolate(sampleValue, 0, sampleValueMax, _decodeValues[d], _decodeValues[d + 1]);

                // Move to next output sample
                bitsOffset += _bitsPerSampleValue;
            }

            return outputs;
        }

        protected override void Initialize()
        {
            base.Initialize();

            _sizeValues = Size.AsNumberArray();

            _samplesValues = new int[_sizeValues.Length];
            _samplesValues[0] = 1;
            for (int i = 1; i < _sizeValues.Length; i++)
                _samplesValues[i] = (int)(_samplesValues[i - 1] * _sizeValues[i]);

            _bitsPerSampleValue = BitsPerSample.Value;
            _orderValue = (Order != null) ? Order.Value : 1;

            if (Encode != null)
                _encodeValues = Encode.AsNumberArray();
            else
            {
                _encodeValues = new float[_sizeValues.Length];
                for (int i = 0, j = 1; i < _sizeValues.Length; i++, j += 2)
                    _encodeValues[j] = _sizeValues[i] - 1;
            }

            if (Decode != null)
                _decodeValues = Decode.AsNumberArray();
            else
                _decodeValues = _rangeValues;
        }
    }
}
