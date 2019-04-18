using System;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class PdfArray : PdfObject
    {
        private ParseArray _array;
        private List<PdfObject> _wrapped;

        public PdfArray(PdfObject parent, ParseArray array)
            : base(parent, array)
        {
            _array = array;
        }

        public List<PdfObject> Objects
        {
            get
            {
                if (_wrapped == null)
                {
                    _wrapped = new List<PdfObject>();
                    foreach (ParseObject obj in _array.Objects)
                        _wrapped.Add(WrapObject(obj));
                }

                return _wrapped;
            }
        }
    }
}
