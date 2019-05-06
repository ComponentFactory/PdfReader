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
        
        public void Process()
        {
            // If CropBox is not specified then default to the mandatory MediaBox
            PdfRectangle cropBox = _page.CropBox;
            if (cropBox == null)
                cropBox = _page.MediaBox;

            Initialize(cropBox);

            PdfContentsParser parser = _page.Contents.CreateParser();
            PdfObject obj = null;

            do
            {
                obj = parser.GetObject();

            } while (obj != null);
        }
    }
}
