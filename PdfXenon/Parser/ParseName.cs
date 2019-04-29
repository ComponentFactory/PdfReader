using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PdfXenon.Standard
{
    public class ParseName : ParseObject
    {
        private static Dictionary<string, ParseName> _lookup = new Dictionary<string, ParseName>();

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

        public static ParseName GetParse(string name)
        {
            if (!_lookup.TryGetValue(name, out ParseName parseName))
            {
                parseName = new ParseName(name);
                _lookup.Add(name, parseName);
            }

            return parseName;
        }
    }
}
