using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseArray : ParseObject
    {
        public ParseArray(long position, List<ParseObject> objects)
            : base(position)
        {
            Objects = objects;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            sb.Append("[");
            indent++;

            int index = 0;
            int count = Objects.Count;
            foreach (ParseObject obj in Objects)
            {
                indent += obj.Output(sb, indent);

                if (index < (count - 1))
                    sb.Append(" ");

                index++;
                indent++;
            }

            sb.Append("]");
            indent++;

            return indent;
        }

        public List<ParseObject> Objects { get; set; }
    }
}
