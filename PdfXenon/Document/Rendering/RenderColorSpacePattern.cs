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

        public override void Visit(IRenderObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int NumberOfComponents()
        {
            throw new ArgumentOutOfRangeException($"Pattern should never be asked for number of components.");
        }

        public override void Parse(float[] values)
        {
            throw new NotImplementedException($"Pattern parse of numbers is not implemented.");
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
                            PdfDictionary shading = dictionary.MandatoryValueRef<PdfDictionary>("Shading");
                            PdfDictionary extGState = dictionary.OptionalValueRef<PdfDictionary>("ExtGState");
                            PdfArray matrix = dictionary.OptionalValueRef<PdfArray>("Matrix");
                            _patten = RenderPatternShading.ParseShading(this, shading, extGState, matrix);
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Pattern provided as a '{obj.GetType().Name}' not implemented.");
                }
            }
            else if (obj is PdfStream stream)
            {
                // Pattern implementation comes from the type
                PdfInteger patternType = stream.Dictionary.MandatoryValue<PdfInteger>("PatternType");
                switch (patternType.Value)
                {
                    case 1: // Tiling Pattern
                        {
                        }
                        break;
                    default:
                        throw new NotImplementedException($"Pattern provided as a '{obj.GetType().Name}' not implemented.");
                }
            }
            else
                throw new NotImplementedException($"Pattern provided as a '{obj.GetType().Name}' not implemented.");
        }

        public RenderPatternType GetPattern()
        {
            return _patten;
        }
    }
}
