using System;

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

        public override string ToString()
        {
            return $"({GetType().Name})";
        }

        public ParseObject ParseObject { get; private set; }
        public PdfObject Parent { get; private set; }
        public PdfDocument Document { get => TypedParent<PdfDocument>(); }

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
            if (obj is ParseString)
                return new PdfString(this, obj as ParseString);
            if (obj is ParseName)
                return new PdfName(this, obj as ParseName);
            else if (obj is ParseInteger)
                return new PdfInteger(this, obj as ParseInteger);
            else if (obj is ParseReal)
                return new PdfReal(this, obj as ParseReal);
            else if (obj is ParseDictionary)
                return new PdfDictionary(this, obj as ParseDictionary);
            else if (obj is ParseObjectReference)
                return new PdfObjectReference(this, obj as ParseObjectReference);
            else if (obj is ParseStream)
                return new PdfStream(this, obj as ParseStream);
            else if (obj is ParseArray)
                return new PdfArray(this, obj as ParseArray);

            throw new ApplicationException($"Cannot wrap object '{obj.GetType().Name}' as a pdf object .");
        }
    }
}
