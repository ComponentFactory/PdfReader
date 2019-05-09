using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfPatternType : PdfObject
    {
        public PdfPatternType(PdfObject parent)
            : base(parent)
        {
        }

        public PdfRenderer Renderer { get => TypedParent<PdfRenderer>(); }
    }
}
