using System;

namespace PdfXenon.Standard
{
    public class PdfPageInherit : PdfDictionary
    {
        public PdfPageInherit(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public PdfPageInherit Inherit { get => TypedParent<PdfPageInherit>(); }

        public T InheritableMandatoryValue<T>(string name) where T : PdfObject
        {
            // Try and get the value from this dictionary
            T here = OptionalValue<T>(name);

            // If not present then inherit it from the parent
            if (here == null)
                here = Inherit.InheritableMandatoryValue<T>(name);

            // Enforce mandatory existence
            if (here == null)
                throw new ApplicationException($"Page is missing a mandatory inheritable value for '{name}'.");

            return here;
        }
    }
}
