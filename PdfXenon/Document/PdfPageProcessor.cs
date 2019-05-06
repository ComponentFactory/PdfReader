using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfPageProcessor : PdfProcessor
    {
        private PdfPage _page;

        public PdfPageProcessor(PdfPage page)
            : base(page)
        {
            _page = page;
        }

        public override PdfDictionary Resources
        {
            get { return _page.Resources; }
        }

        public void Process()
        {
            PdfContentsParser parser = _page.Contents.CreateParser();
            PdfObject obj = null;

            do
            {
                obj = parser.GetObject();
                if (obj != null)
                    Process(obj);

            } while (obj != null);
        }
    }
}
