using System;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class ParseObject
    {
        public ParseObject(long position)
        {
            Position = position;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Output(sb, 0);
            return sb.ToString();
        }

        public abstract int Output(StringBuilder sb, int indent);

        public long Position { get; protected set; }
    }
}
