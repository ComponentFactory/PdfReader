using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfProcessor : PdfObject
    {
        private Stack<PdfObject> _stack = new Stack<PdfObject>();

        public PdfProcessor(PdfObject parent)
            : base(parent)
        {
        }

        public void Initialize(PdfRectangle cropBox)
        {
        }

        public void Process(PdfObject obj)
        {
        }
    }
}
