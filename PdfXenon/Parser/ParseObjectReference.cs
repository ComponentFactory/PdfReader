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

        public int Id { get; private set; }
        public int Gen { get; private set; }
    }
}
