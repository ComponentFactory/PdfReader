using System.Text;

namespace PdfXenon.Standard
{
    public class ParseReal : ParseObject
    {
        public ParseReal(TokenReal token)
        {
            Value = token.Value;
        }

        public float Value { get; private set; }
    }
}
