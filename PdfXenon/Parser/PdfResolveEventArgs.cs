using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfResolveEventArgs : EventArgs
    {
        public int Id { get; set; }
        public int Gen { get; set; }
        public PdfObject Object { get; set; }
    }
}
