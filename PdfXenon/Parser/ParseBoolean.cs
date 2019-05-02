using System.Text;

namespace PdfXenon.Standard
{
    public class ParseBoolean : ParseObject
    {
        public ParseBoolean(bool value)
        {
            Value = value;
        }

        public bool Value { get; private set; }
    }
}
