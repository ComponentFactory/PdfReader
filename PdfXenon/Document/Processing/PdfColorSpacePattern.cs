using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfColorSpacePattern : PdfColorSpace
    {
        private PdfPatternType _patten;

        public PdfColorSpacePattern(PdfRenderer renderer)
            : base(renderer)
        {
        }

        public override void ParseParameters()
        {
            string patternName = Renderer.OperandAsString();

            // Resolve the pattern name space to an object using the resolver
            PdfObject obj = Renderer.Resolver.GetPatternObject(patternName);
            if (obj is PdfDictionary dictionary)
            {
                // Pattern implementation comes from the type
                PdfInteger patternType = dictionary.MandatoryValue<PdfInteger>("PatternType");
                switch (patternType.Value)
                {
                    case 2: // Shading
                        {
                            PdfDictionary shading = dictionary.MandatoryValue<PdfDictionary>("Shading");
                            PdfInteger shadingType = shading.MandatoryValue<PdfInteger>("ShadingType");
                            switch (shadingType.Value)
                            {
                                case 2:
                                    _patten = new PdfPatternShadingAxial(Renderer, shading);
                                    return;
                                default:
                                    throw new NotImplementedException($"Pattern shading type '{shadingType.Value}' not implemented.");
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Pattern type '{patternType.Value}' not implemented.");
                }
            }
            else if (obj is PdfStream stream)
            {
                throw new NotImplementedException($"Pattern provided as a stream is not implemented.");
            }
        }

        public override bool IsPattern { get => true; }

        public override PdfPatternType GetPattern()
        {
            return _patten;
        }
    }
}
