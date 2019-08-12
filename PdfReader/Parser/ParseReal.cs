using System.Text;

namespace PdfReader
{
    public class ParseReal : ParseObjectBase
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
