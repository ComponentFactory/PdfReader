using System.Text;

namespace PdfReader
{
    public class ParseBoolean : ParseObjectBase
    {
        public ParseBoolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; private set; }
    }
}
