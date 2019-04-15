using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfPages : PdfPageInherit
    {
        public PdfPages(PdfPages parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
            Parent = parent;
        }

        public PdfPages Parent { get; private set; }

        public void CreatePages(PdfDocument document, List<PdfPage> pages)
        {
            ParseArray kids = MandatoryValue<ParseArray>("Kids");
            foreach(ParseObjectReference reference in kids.Objects)
            {
                ParseDictionary dictionary = document.IndirectObjects.MandatoryValue<ParseDictionary>(reference);
                string type = dictionary.MandatoryValue<ParseName>("Type").Value;

                if (type == "Page")
                {
                    PdfPage pdfPage = new PdfPage(this, dictionary);
                    pages.Add(pdfPage);
                }
                else if (type == "Pages")
                {
                    PdfPages pdfPages = new PdfPages(this, dictionary);
                    pdfPages.CreatePages(document, pages);
                }
            }
        }
    }
}
