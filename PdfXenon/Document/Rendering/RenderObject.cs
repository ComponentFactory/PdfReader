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
        public Renderer Renderer { get => TypedParent<Renderer>(); }

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

        public bool AsBoolean(PdfObject obj)
        {
            if (obj is PdfBoolean boolean)
                return boolean.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a boolean.");
        }

        public string AsString(PdfObject obj)
        {
            if (obj is PdfName name)
                return name.Value;
            else if (obj is PdfString str)
                return str.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a string.");
        }

        public int AsInteger(PdfObject obj)
        {
            if (obj is PdfInteger integer)
                return integer.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected an integer.");
        }

        public float AsNumber(PdfObject obj)
        {
            if (obj is PdfInteger integer)
                return integer.Value;
            else if (obj is PdfReal real)
                return real.Value;

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected a number.");
        }

        public float[] AsNumberArray(PdfObject obj)
        {
            if (obj is PdfArray array)
            {
                List<float> numbers = new List<float>();
                foreach (PdfObject item in array.Objects)
                {
                    if (item is PdfInteger integer)
                        numbers.Add(integer.Value);
                    else if (item is PdfReal real)
                        numbers.Add(real.Value);
                    else
                        throw new ApplicationException($"Array contains object of type '{obj.GetType().Name}', expected only numbers.");

                }

                return numbers.ToArray();
            }

            throw new ApplicationException($"Unexpected object in content '{obj.GetType().Name}', expected an integer array.");
        }
    }
}
