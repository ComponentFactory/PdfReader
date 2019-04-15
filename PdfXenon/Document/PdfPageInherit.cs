using System;

namespace PdfXenon.Standard
{
    public class PdfPageInherit : PdfDictionary
    {
        public PdfPageInherit(PdfPageInherit inherit, ParseDictionary dictionary)
            : base(dictionary)
        {
            Inherit = inherit;
        }

        public PdfPageInherit Inherit { get; private set; }

        public int Count
        {
            get { return MandatoryValue<ParseInteger>("Count").Value; }
        }

        public T MandatoryInheritableValue<T>(string name) where T : ParseObject
        {
            // Try and get the value from this dictionary
            T here = OptionalValue<T>(name);

            // If not present then inherit it from the parent
            if (here == null)
                here = Inherit.MandatoryInheritableValue<T>(name);

            // Enforce mandatory existence
            if (here == null)
                throw new ApplicationException($"Page is missing a mandatory, inheritable value for '{name}'.");

            return here;
        }
    }
}
