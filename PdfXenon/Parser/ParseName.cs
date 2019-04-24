using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PdfXenon.Standard
{
    public class ParseName : ParseObject
    {
        private static Dictionary<string, string> _unique = new Dictionary<string, string>();

        public ParseName(TokenName token)
            : base(token.Position)
        {
            // Only keep a single instance of the same Name value
            if (_unique.TryGetValue(token.Value, out string unique))
                Value = unique;
            else
            {
                Value = token.Value;
                _unique.Add(token.Value, token.Value);
            }
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
