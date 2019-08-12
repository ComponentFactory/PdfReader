using System;

namespace PdfReader
{
    public class ParseResolveEventArgs : EventArgs
    {
        public int Id { get; set; }
        public int Gen { get; set; }
        public ParseObjectBase Object { get; set; }
    }
}
