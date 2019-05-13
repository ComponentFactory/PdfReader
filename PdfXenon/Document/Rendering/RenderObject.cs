using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderObject
    {
        public RenderObject(RenderObject parent)
        {
            Parent = parent;
        }

        public RenderObject Parent { get; private set; }

        public virtual void Visit(IRenderObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Renderer Renderer
        {
            get
            {
                if (this is Renderer)
                    return (Renderer)this;
                else
                    return TypedParent<Renderer>();
            }

        }

        public T TypedParent<T>() where T : RenderObject
        {
            RenderObject parent = Parent;

            while (parent != null)
            {
                if (parent is T)
                    return parent as T;
                else
                    parent = parent.Parent;
            }

            return null;
        }
    }
}
