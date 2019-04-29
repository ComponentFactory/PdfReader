using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseIdentifier : ParseObject
    {
        private static ConcurrentDictionary<string, ParseIdentifier> _lookup = new ConcurrentDictionary<string, ParseIdentifier>();
        private static Func<string, ParseIdentifier, ParseIdentifier> _nullUpdate = (x, y) => y;

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
                _lookup.AddOrUpdate(identifier, parseIdentifier, _nullUpdate);
            }

            return parseIdentifier;
        }
    }
}
