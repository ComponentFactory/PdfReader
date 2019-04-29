using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseIdentifier : ParseObject
    {
        private static Dictionary<string, ParseIdentifier> _lookup = new Dictionary<string, ParseIdentifier>();

        public ParseIdentifier(string value)
        {
            Value = value;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value;
            sb.Append(output);
            return indent + output.Length;
        }

        public string Value { get; private set; }

        public static ParseIdentifier GetParse(string identifier)
        {
            if (!_lookup.TryGetValue(identifier, out ParseIdentifier parseIdentifier))
            {
                parseIdentifier = new ParseIdentifier(identifier);
                _lookup.Add(identifier, parseIdentifier);
            }

            return parseIdentifier;
        }
    }
}
