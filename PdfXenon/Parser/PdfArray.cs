using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfArray : PdfObject
    {
        public PdfArray(long position, List<PdfObject> objects)
        {
            ArrayOpenPosition = position;
            Objects = objects;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"PdfArray: Length:{Objects.Count} (");

            foreach (PdfObject obj in Objects)
                sb.AppendLine($"    {obj.ToString()}");

            sb.AppendLine(")");

            return sb.ToString();
        }

        public override long Position { get => ArrayOpenPosition; }
        public List<PdfObject> Objects { get; set; }

        private long ArrayOpenPosition { get; set; }
    }
}
