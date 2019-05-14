using System.Text;

namespace PdfXenon.Standard
{
    public class ParseReal : ParseObject
    {
        public ParseReal(TokenReal token)
            : this(token.Value)
        {
        }

        public ParseReal(float value)
        {
            Value = value;
        }

        public float Value { get; private set; }
    }
}
