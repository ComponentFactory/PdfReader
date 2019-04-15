using System;

namespace PdfXenon.Standard
{
    public class PdfPage : PdfPageInherit
    {
        private PdfDictionary _resources;

        public PdfPage(PdfPages parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
            Parent = parent;
        }

        public PdfPages Parent { get; private set; }

        public PdfDictionary Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new PdfDictionary(MandatoryInheritableValue<ParseDictionary>("Resources"));

                return _resources;
            }
        }
    }
}
