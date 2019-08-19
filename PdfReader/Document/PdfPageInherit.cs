using System;
using System.Collections.Generic;

namespace PdfReader
{
    public abstract class PdfPageInherit : PdfDictionary
    {
        public PdfPageInherit(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfPageInherit Inherit { get => TypedParent<PdfPageInherit>(); }
        public abstract void FindLeafPages(List<PdfPage> pages);

        public T InheritableOptionalValue<T>(string name) where T : PdfObject
        {
            // Try and get the value from this dictionary
            T here = OptionalValue<T>(name);

            // If not present then inherit it from the parent
            if ((here == null) && (Inherit != null))
                here = Inherit.InheritableOptionalValue<T>(name);

            return here;
        }

        public T InheritableOptionalRefValue<T>(string name) where T : PdfObject
        {
            // Try and get the value from this dictionary
            T here = OptionalValueRef<T>(name);

            // If not present then inherit it from the parent
            if ((here == null) && (Inherit != null))
                here = Inherit.InheritableOptionalRefValue<T>(name);

            return here;
        }

        public T InheritableMandatoryValue<T>(string name) where T : PdfObject
        {
            // Try and get the value from this dictionary
            T here = OptionalValue<T>(name);

            // If not present then inherit it from the parent
            if ((here == null) && (Inherit != null))
                here = Inherit.InheritableMandatoryValue<T>(name);

            // Enforce mandatory existence
            if (here == null)
                throw new ApplicationException($"Page is missing a mandatory inheritable value for '{name}'.");

            return here;
        }

        public T InheritableMandatoryRefValue<T>(string name) where T : PdfObject
        {
            // Try and get the value from this dictionary
            T here = OptionalValueRef<T>(name);

            // If not present then inherit it from the parent
            if ((here == null) && (Inherit != null))
                here = Inherit.InheritableMandatoryRefValue<T>(name);

            // Enforce mandatory existence
            if (here == null)
                throw new ApplicationException($"Page is missing a mandatory inheritable value for '{name}'.");

            return here;
        }
    }
}
