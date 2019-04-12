using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseArray : ParseObject
    {
        public ParseArray(long position, List<ParseObject> objects)
        {
            ArrayOpenPosition = position;
            Objects = objects;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"ParseArray ({Position}): Count:{Objects.Count} (");

            foreach (ParseObject obj in Objects)
                sb.AppendLine($"    {obj.ToString()}");

            sb.AppendLine(")");

            return sb.ToString();
        }

        public override long Position { get => ArrayOpenPosition; }
        public List<ParseObject> Objects { get; set; }

        private long ArrayOpenPosition { get; set; }
    }
}
