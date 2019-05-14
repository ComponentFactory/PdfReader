using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public interface IRendererResolver
    {
        PdfStream GetStream(PdfObjectReference reference);
        PdfDictionary GetGraphicsStateDictionary(string dictName);
        PdfObject GetColorSpaceObject(string colorSpaceName);
        PdfObject GetPatternObject(string patternName);
        PdfDictionary GetShadingObject(string shadingName);
    }
}
