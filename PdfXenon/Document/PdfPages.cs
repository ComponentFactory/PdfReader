using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfPages : PdfPageInherit
    {
        public PdfPages(PdfDocument doc, PdfPages parent, ParseDictionary dictionary)
            : base(doc, parent, dictionary)
        {
        }

        public PdfPages Parent { get => (Inherit as PdfPages); }

        public void CreatePages(PdfDocument document, List<PdfPage> pages)
        {
            ParseArray kids = MandatoryValue<ParseArray>("Kids");
            foreach(ParseObjectReference reference in kids.Objects)
            {
                ParseDictionary dictionary = document.IndirectObjects.MandatoryValue<ParseDictionary>(reference);
                string type = dictionary.MandatoryValue<ParseName>("Type").Value;

                if (type == "Page")
                {
                    PdfPage pdfPage = new PdfPage(Doc, this, dictionary);
                    pages.Add(pdfPage);
                }
                else if (type == "Pages")
                {
                    PdfPages pdfPages = new PdfPages(Doc, this, dictionary);
                    pdfPages.CreatePages(document, pages);
                }
            }
        }
    }
}
