using System.Text;

namespace PdfReader
{
    public class ParseIndirectObject : ParseObjectBase
    {
        public ParseIndirectObject(TokenInteger id, TokenInteger gen, ParseObjectBase obj)
        {
            Id = id.Value;
            Gen = gen.Value;
            Object = obj;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public ParseObjectBase Object { get; private set; }
    }
}
