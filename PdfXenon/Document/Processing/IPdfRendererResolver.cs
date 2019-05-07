using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public interface IPdfRendererResolver
    {
        PdfDictionary GetGraphicsStateDictionary(string dictName);
    }
}
