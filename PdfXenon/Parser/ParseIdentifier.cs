using System.Text;

namespace PdfXenon.Standard
{
    public class ParseIdentifier : ParseObject
    {
        public ParseIdentifier(TokenIdentifier token)
            : base(token.Position)
        {
            Value = token.Value;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value;
            sb.Append(output);
            return indent + output.Length;
        }

        public string Value { get; private set; }
    }
}
