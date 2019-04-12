using System;

namespace PdfXenon.Standard
{
    public class ParseResolveEventArgs : EventArgs
    {
        public int Id { get; set; }
        public int Gen { get; set; }
        public ParseObject Object { get; set; }
    }
}
