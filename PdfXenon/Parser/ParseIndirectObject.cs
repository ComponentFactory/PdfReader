using System.Text;

namespace PdfXenon.Standard
{
    public class ParseIndirectObject : ParseObject
    {
        public ParseIndirectObject(TokenInteger id, TokenInteger gen, ParseObject obj)
            : base(id.Position)
        {
            Id = id.Value;
            Gen = gen.Value;
            Object = obj;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string prefix = $"{Id} {Gen} obj\n";
            sb.Append(prefix);

            string blank = new string(' ', indent);
            sb.Append(blank);
            Object.Output(sb, indent);

            sb.Append("\n");
            sb.Append(blank);
            sb.Append("endobj\n");
            sb.Append(blank);

            return indent;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public ParseObject Object { get; private set; }
    }
}
