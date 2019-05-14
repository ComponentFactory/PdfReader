using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderPageResolver : IRendererResolver
    {
        public RenderPageResolver(PdfPage page, Renderer renderer)
        {
            Page = page;
            Renderer = renderer;
            Renderer.Resolver = this;

            // MediaBox is mandatory but CropBox inherits from MediaBox if not present
            PdfRectangle mediaBox = Page.MediaBox;
            PdfRectangle cropBox = Page.CropBox;
            if (cropBox == null)
                cropBox = Page.MediaBox;

            Renderer.Initialize(mediaBox, cropBox);
        }

        public PdfPage Page { get; private set; }
        public Renderer Renderer { get; private set; }

        public PdfStream GetStream(PdfObjectReference reference)
        {
            return (PdfStream)Page.Document.ResolveReference(reference);
        }

        public PdfDictionary GetGraphicsStateDictionary(string dictName)
        {
            PdfDictionary extGStates = Page.Resources.MandatoryValueRef<PdfDictionary>("ExtGState");
            return extGStates.OptionalValueRef<PdfDictionary>(dictName);
        }

        public PdfObject GetColorSpaceObject(string colorSpaceName)
        {
            PdfDictionary colorSpaces = Page.Resources.MandatoryValueRef<PdfDictionary>("ColorSpace");
            return colorSpaces.OptionalValueRef<PdfObject>(colorSpaceName);
        }

        public PdfObject GetPatternObject(string patternName)
        {
            PdfDictionary patterns = Page.Resources.MandatoryValueRef<PdfDictionary>("Pattern");
            return patterns.OptionalValueRef<PdfObject>(patternName);
        }

        public PdfDictionary GetShadingObject(string shadingName)
        {
            PdfDictionary patterns = Page.Resources.MandatoryValueRef<PdfDictionary>("Shading");
            return patterns.OptionalValueRef<PdfDictionary>(shadingName);
        }

        public void Process()
        {
            PdfContentsParser parser = Page.Contents.CreateParser();
            PdfObject obj = null;

            do
            {
                obj = parser.GetObject();
                if (obj != null)
                    Renderer.Render(obj);

            } while (obj != null);
        }
    }
}
