using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderColorSpacePattern : RenderColorSpace
    {
        private RenderPatternType _patten;

        public RenderColorSpacePattern(RenderObject parent)
            : base(parent)
        {
        }

        public override void ParseParameters()
        {
            string patternName = Renderer.OperandAsString();

            // Resolve the pattern name to an object using the resolver
            PdfObject obj = Renderer.Resolver.GetPatternObject(patternName);
            if (obj is PdfDictionary dictionary)
            {
                // Pattern implementation comes from the type
                PdfInteger patternType = dictionary.MandatoryValue<PdfInteger>("PatternType");
                switch (patternType.Value)
                {
                    case 2: // Shading Pattern
                        {
                            PdfDictionary shading = dictionary.MandatoryValue<PdfDictionary>("Shading");
                            PdfInteger shadingType = shading.MandatoryValue<PdfInteger>("ShadingType");
                            switch (shadingType.Value)
                            {
                                case 2: // Axial Shading
                                    _patten = new RenderPatternShadingAxial(this, shading);
                                    return;
                                default:
                                    throw new NotImplementedException($"Pattern shading type '{shadingType.Value}' not implemented.");
                            }
                        }
                    default:
                        throw new NotImplementedException($"Pattern type '{patternType.Value}' not implemented.");
                }
            }
            else if (obj is PdfStream stream)
            {
                throw new NotImplementedException($"Pattern provided as a stream is not implemented.");
            }
        }

        public RenderPatternType GetPattern()
        {
            return _patten;
        }
    }
}
