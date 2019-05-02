using System.Text;

namespace PdfXenon.Standard
{
    public class ParseInteger : ParseObject
    {
        public ParseInteger(TokenInteger token)
        {
            Value = token.Value;
        }

        public int Value { get; private set; }
    }
}
