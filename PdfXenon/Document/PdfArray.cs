using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfArray : PdfObject
    {
        private List<PdfObject> _wrapped;

        public PdfArray(PdfObject parent, ParseArray array)
            : base(parent, array)
        {
        }

        public override int Output(StringBuilder sb, int indent)
        {
            sb.Append("[");
            indent++;

            int index = 0;
            int count = Objects.Count;
            foreach (PdfObject obj in Objects)
            {
                indent += obj.Output(sb, indent);

                if (index < (count - 1))
                    sb.Append(" ");

                index++;
                indent++;
            }

            sb.Append("]");
            indent++;

            return indent;
        }

        public ParseArray ParseArray { get => ParseObject as ParseArray; }

        public List<PdfObject> Objects
        {
            get
            {
                if (_wrapped == null)
                {
                    _wrapped = new List<PdfObject>();
                    foreach (ParseObject obj in ParseArray.Objects)
                        _wrapped.Add(WrapObject(obj));
                }

                return _wrapped;
            }
        }
    }
}
