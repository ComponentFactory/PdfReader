using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfPages : PdfPageInherit
    {
        public PdfPages(PdfObject parent, ParseDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public void CreatePages(List<PdfPage> pages)
        {
            PdfArray kids = MandatoryValue<PdfArray>("Kids");
            foreach(PdfObjectReference reference in kids.Objects)
            {
                PdfDictionary dictionary = Document.IndirectObjects.MandatoryValue<PdfDictionary>(reference);
                string type = dictionary.MandatoryValue<PdfName>("Type").Value;

                if (type == "Page")
                {
                    PdfPage pdfPage = new PdfPage(this, dictionary.ParseObject as ParseDictionary);
                    pages.Add(pdfPage);
                }
                else if (type == "Pages")
                {
                    PdfPages pdfPages = new PdfPages(this, dictionary.ParseObject as ParseDictionary);
                    pdfPages.CreatePages(pages);
                }
            }
        }
    }
}
