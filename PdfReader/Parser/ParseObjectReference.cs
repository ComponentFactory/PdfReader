using System.Text;

namespace PdfReader
{
    public class ParseObjectReference : ParseObjectBase
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
