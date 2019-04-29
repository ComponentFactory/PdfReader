using System;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class ParseObject
    {
        public static readonly ParseBoolean True = new ParseBoolean(true);
        public static readonly ParseBoolean False = new ParseBoolean(false);
        public static readonly ParseNull Null = new ParseNull();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Output(sb, 0);
            return sb.ToString();
        }

        public abstract int Output(StringBuilder sb, int indent);
    }
}
