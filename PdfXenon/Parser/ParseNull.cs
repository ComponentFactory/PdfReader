using System.Text;

namespace PdfXenon.Standard
{
    public class ParseNull : ParseObject
    {
        public ParseNull()
        {
        }

        public override int Output(StringBuilder sb, int indent)
        {
            sb.Append("null");
            return indent + 4;
        }
    }
}
