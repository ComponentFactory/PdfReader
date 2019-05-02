using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseArray : ParseObject
    {
        public ParseArray(List<ParseObject> objects)
        {
            Objects = objects;
        }

        public List<ParseObject> Objects { get; set; }
    }
}
