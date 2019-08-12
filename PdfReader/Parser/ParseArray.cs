using System.Collections.Generic;
using System.Text;

namespace PdfReader
{
    public class ParseArray : ParseObjectBase
    {
        public ParseArray(List<ParseObjectBase> objects)
        {
            Objects = objects;
        }

        public List<ParseObjectBase> Objects { get; set; }
    }
}
