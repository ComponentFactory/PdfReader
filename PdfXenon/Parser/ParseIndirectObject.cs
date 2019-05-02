using System.Text;

namespace PdfXenon.Standard
{
    public class ParseIndirectObject : ParseObject
    {
        public ParseIndirectObject(TokenInteger id, TokenInteger gen, ParseObject obj)
        {
            Id = id.Value;
            Gen = gen.Value;
            Object = obj;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public ParseObject Object { get; private set; }
    }
}
