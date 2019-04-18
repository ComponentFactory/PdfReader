using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfContents : PdfObject
    {
        private List<ParseStream> _streams = new List<ParseStream>();

        public PdfContents(PdfObject parent, ParseObject obj)
            : base(parent, obj)
        {
            ResolveToStreams(obj);
        }

        private void ResolveToStreams(ParseObject obj)
        {
            if (obj is ParseStream)
                _streams.Add(obj as ParseStream);
            else if (obj is ParseObjectReference)
            {
                ResolveToStreams(Document.ResolveReference(new PdfObjectReference(this, obj as ParseObjectReference)).ParseObject);
            }
            else if (obj is ParseArray)
            {
                ParseArray array = (ParseArray)obj;
                foreach (ParseObject entry in array.Objects)
                    ResolveToStreams(entry);
            }
        }
    }
}
