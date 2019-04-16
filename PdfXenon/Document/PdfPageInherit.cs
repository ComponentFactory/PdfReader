using System;

namespace PdfXenon.Standard
{
    public class PdfPageInherit : PdfDictionary
    {
        public PdfPageInherit(PdfDocument doc, PdfPageInherit inherit, ParseDictionary parse)
            : base(doc, parse)
        {
            Inherit = inherit;
        }

        public PdfPageInherit Inherit { get; private set; }

        public T InheritableMandatoryValue<T>(string name) where T : ParseObject
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
