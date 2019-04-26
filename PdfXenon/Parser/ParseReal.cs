using System.Text;

namespace PdfXenon.Standard
{
    public class ParseReal : ParseObject
    {
        public ParseReal(TokenReal token)
        {
            Value = token.Value;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value.ToString();
            sb.Append(output);
            return indent + output.Length;
        }

        public float Value { get; private set; }
    }
}
