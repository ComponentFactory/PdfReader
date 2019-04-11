using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ResolveEventArgs : EventArgs
    {
        public PdfObjectReference Reference { get; set; }
        public PdfObject Object { get; set; }
    }
}
