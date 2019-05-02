using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfIndirectObject : PdfObject
    {
        public PdfIndirectObject(PdfObject parent, TokenXRefEntry xref)
            : base(parent)
        {
            Id = xref.Id;
            Gen = xref.Gen;
            Offset = xref.Offset;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);

            sb.Append($"{Id} {Gen} obj\n");
            Child.Output(sb, indent);
            sb.Append("\n");
            sb.Append(blank);
            sb.Append("endobj\n");
            sb.Append(blank);
            return indent;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public long Offset { get; private set; }
        public PdfObject Child { get; set; }
    }
}