using System.Text;

namespace PdfXenon.Standard
{
    public class ParseObjectReference : ParseObject
    {
        public ParseObjectReference(TokenInteger id, TokenInteger gen)
        {
            Id = id.Value;
            Gen = gen.Value;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = $"{Id} {Gen} R";
            sb.Append(output);
            return indent + output.Length;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
    }
}
