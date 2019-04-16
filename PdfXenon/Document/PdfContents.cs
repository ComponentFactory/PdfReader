using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfContents : PdfObject
    {
        private List<ParseStream> _streams = new List<ParseStream>();

        public PdfContents(PdfDocument doc, ParseObject parse)
            : base(doc)
        {
            ResolveToStreams(parse);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"PdfContent\n");

            foreach (ParseStream stream in _streams)
                sb.AppendLine(stream.ToString());

            return sb.ToString();
        }

        private void ResolveToStreams(ParseObject parse)
        {
            if (parse is ParseStream)
                _streams.Add(parse as ParseStream);
            else if (parse is ParseObjectReference)
                ResolveToStreams(Doc.ResolveReference(parse as ParseObjectReference));
            else if (parse is ParseArray)
            {
                ParseArray array = (ParseArray)parse;
                foreach (ParseObject entry in array.Objects)
                    ResolveToStreams(entry);
            }
        }
    }
}
