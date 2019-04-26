using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PdfXenon.Standard
{
    public class ParseName : ParseObject
    {
        public ParseName(string value)
        {
            Value = value;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = $"/{Value}";
            sb.Append(output);
            return indent + output.Length;
        }

        public string Value { get; private set; }
    }
}
