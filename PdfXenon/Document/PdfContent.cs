using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfContent : PdfObject
    {
        private List<ParseStream> _streams = new List<ParseStream>();

        public PdfContent(PdfDocument doc, ParseObject content)
            : base(doc)
        {
            //ParseObjectReference reference = contents as ParseObjectReference;
            //if (reference != null)
            //    contents = Doc.ResolveReference(reference.Id, reference.Gen);

            //ParseStream stream = content as ParseStream;
            //ParseArray array = content as ParseArray;

            //if (stream != null)
            //    _streams.Add(stream);
            //else if (array != null)
            //{
            //    throw new ApplicationException("Array of streams in page 'Contents' not yet implemented.");
            //}
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"PdfContent\n");

            foreach (ParseStream stream in _streams)
                sb.AppendLine(stream.ToString());

            return sb.ToString();
        }
    }
}
