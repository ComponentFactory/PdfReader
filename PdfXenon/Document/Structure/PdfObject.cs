using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfObject
    {
        public PdfObject(PdfObject parent)
            : this(parent, null)
        {
        }

        public PdfObject(PdfObject parent, ParseObject parse)
        {
            Parent = parent;
            ParseObject = parse;
        }

        public virtual void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public PdfObject Parent { get; private set; }
        public ParseObject ParseObject { get; private set; }
        public PdfDocument Document { get => TypedParent<PdfDocument>(); }
        public PdfDecrypt Decrypt { get => TypedParent<PdfDocument>().DecryptHandler; }

        public T TypedParent<T>() where T : PdfObject
        {
            PdfObject parent = Parent;

            while (parent != null)
            {
                if (parent is T)
                    return parent as T;
                else
                    parent = parent.Parent;
            }

            return null;
        }

        public PdfObject WrapObject(ParseObject obj)
        {
            if (obj is ParseString str)
                return new PdfString(this, str);
            if (obj is ParseName name)
                return new PdfName(this, name);
            else if (obj is ParseInteger integer)
                return new PdfInteger(this, integer);
            else if (obj is ParseReal real)
                return new PdfReal(this, real);
            else if (obj is ParseDictionary dictionary)
                return new PdfDictionary(this, dictionary);
            else if (obj is ParseObjectReference reference)
                return new PdfObjectReference(this, reference);
            else if (obj is ParseStream stream)
                return new PdfStream(this, stream);
            else if (obj is ParseArray array)
                return new PdfArray(this, array);
            else if (obj is ParseIdentifier identifier)
                return new PdfIdentifier(this, identifier);
            else if (obj is ParseBoolean boolean)
                return new PdfBoolean(this, boolean);
            if (obj is ParseNull nul)
                return new PdfNull(this);

            throw new ApplicationException($"Cannot wrap object '{obj.GetType().Name}' as a pdf object .");
        }

        public static PdfRectangle ArrayToRectangle(PdfArray array)
        {
            if (array != null)
                return new PdfRectangle(array.Parent, array.ParseArray);
            else
                return null;
        }
    }
}
