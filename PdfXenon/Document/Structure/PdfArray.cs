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

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
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
