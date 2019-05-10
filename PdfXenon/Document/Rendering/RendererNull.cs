using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class RendererNull : Renderer
    {
        public override void Initialize(PdfRectangle mediaBox, PdfRectangle cropBox)
        {
        }

        public override void SubPathStart(RenderPoint pt)
        {
        }

        public override void SubPathLineTo(RenderPoint pt)
        {
        }

        public override void SubPathBezier(RenderPoint pt2, RenderPoint pt3, RenderPoint pt4)
        {
        }

        public override void SubPathClose()
        {
        }

        public override void PathRectangle(RenderPoint pt, float width, float height)
        {
        }

        public override void PathStroke()
        {
        }

        public override void PathFill(bool evenOdd)
        {
        }

        public override void PathClip(bool evenOdd)
        {
        }

        public override void PathEnd()
        {
        }

        public override void Finshed()
        {
        }
    }
}
