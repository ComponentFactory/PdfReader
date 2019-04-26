using System.Text;

namespace PdfXenon.Standard
{
    public class ParseInteger : ParseObject
    {
        public ParseInteger(TokenInteger token)
        {
            Value = token.Value;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value.ToString();
            sb.Append(output);
            return indent + output.Length;
        }

        public int Value { get; private set; }
    }
}
