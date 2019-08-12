using System;
using System.Text;

namespace PdfReader
{
    public abstract class ParseObjectBase
    {
        public static readonly ParseBoolean True = new ParseBoolean(true);
        public static readonly ParseBoolean False = new ParseBoolean(false);
        public static readonly ParseNull Null = new ParseNull();
    }
}
