using System.Text;

namespace PdfXenon.Standard
{
    public class ParseBoolean : ParseObject
    {
        public ParseBoolean(TokenKeyword token)
        {
            Value = (token.Value == ParseKeyword.True);
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value.ToString();
            sb.Append(output);
            return indent + output.Length;
        }

        public bool Value { get; private set; }
    }
}
