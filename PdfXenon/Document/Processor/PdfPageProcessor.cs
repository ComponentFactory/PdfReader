using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfPageProcessor
    {
        private PdfPage _page;
        private PdfProcessor _processor;

        public PdfPageProcessor(PdfPage page, PdfProcessor processor)
        {
            _page = page;
            _processor = processor;

            // Set the page resources into the processor
            _processor.Resources = _page.Resources;
        }

        public void Process()
        {
            // Get the content to process from the page
            PdfContentsParser parser = _page.Contents.CreateParser();

            PdfObject obj = null;

            do
            {
                obj = parser.GetObject();
                if (obj != null)
                    _processor.Process(obj);

            } while (obj != null);
        }
    }
}
