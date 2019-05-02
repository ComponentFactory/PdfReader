using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfOutlineLevel : PdfObject
    {
        public PdfOutlineLevel(PdfObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            Items = new List<PdfOutlineItem>();

            if (dictionary != null)
            {
                PdfDictionary item = dictionary.OptionalValueRef<PdfDictionary>("First");
                while (item != null)
                {
                    Items.Add(new PdfOutlineItem(this, item));
                    item = item.OptionalValueRef<PdfDictionary>("Next");
                }
            }
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string blank = new string(' ', indent);

            foreach (PdfOutlineItem item in Items)
            {
                sb.Append("  ");
                item.Output(sb, indent + 2);
            }

            return indent;
        }

        public int Count { get => Items.Count; }
        public List<PdfOutlineItem> Items { get; private set; }
    }
}
