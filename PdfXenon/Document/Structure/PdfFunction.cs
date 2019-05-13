using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfFunction : PdfObject
    {
        protected float[] _domainValues { get; private set; }
        protected float[] _rangeValues { get; private set; }

        public PdfFunction(PdfObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            Dictionary = dictionary;
            Initialize();
        }

        public PdfDictionary Dictionary { get; private set; }
        public PdfInteger FunctionType { get => Dictionary.MandatoryValue<PdfInteger>("FunctionType"); }
        public PdfArray Domain { get => Dictionary.MandatoryValue<PdfArray>("Domain"); }
        public PdfArray Range { get => Dictionary.OptionalValue<PdfArray>("Range"); }

        public abstract float[] Call(float[] inputs);

        public static PdfFunction FromObject(PdfObject parent, PdfObject obj)
        {
            if (obj is PdfObjectReference referece)
                return FromObject(parent, parent.Document.ResolveReference(referece));
            if (obj is PdfStream stream)
                return FromStream(parent, stream);
            if (obj is PdfDictionary dictionary)
                return FromDictionary(parent, dictionary);

            throw new NotImplementedException($"Function cannot be created from object of type '{obj.GetType().Name}'.");
        }

        public static PdfFunction FromStream(PdfObject parent, PdfStream stream)
        {
            PdfInteger functionType = stream.Dictionary.MandatoryValue<PdfInteger>("FunctionType");
            switch (functionType.Value)
            {
                case 0: // Sampled Function
                    return new PdfFunctionType0(parent, stream);
                default:
                    throw new NotImplementedException($"Function type '{functionType.Value}' not implemented.");
            }
        }

        public static PdfFunction FromDictionary(PdfObject parent, PdfDictionary dictionary)
        {
            PdfInteger functionType = dictionary.MandatoryValue<PdfInteger>("FunctionType");
            switch (functionType.Value)
            {
                case 2: // Exponential Interpolation Function
                    return new PdfFunctionType2(parent, dictionary);
                case 3: // Stitching Function
                    return new PdfFunctionType3(parent, dictionary);
                default:
                    throw new NotImplementedException($"Function type '{functionType.Value}' not implemented.");
            }
        }

        protected virtual void Initialize()
        {
            _domainValues = Domain.AsNumberArray();

            // Range is optional for some types of function
            if (Range != null)
                _rangeValues = Range.AsNumberArray();
        }

        protected float Interpolate(float value, float domain1, float domain2, float range1, float range2)
        {
            return ((value - domain1) * (range2 - range1) / (domain2 - domain1)) + range1;
        }
    }
}
