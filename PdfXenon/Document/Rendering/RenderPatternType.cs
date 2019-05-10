using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderPatternType : RenderObject
    {
        public RenderPatternType(RenderObject parent)
            : base(parent)
        {
        }
    }
}
