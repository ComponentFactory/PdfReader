using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class TokenName : TokenObject
    {
        private static Dictionary<string, TokenName> _lookup = new Dictionary<string, TokenName>();

        public TokenName(string name)
        {
            Value = name;
        }

        public string Value { get; private set; }

        public static TokenName GetToken(string name)
        {
            if (!_lookup.TryGetValue(name, out TokenName tokenName))
            {
                tokenName = new TokenName(name);
                _lookup.Add(name, tokenName);
            }

            return tokenName;
        }
    }
}
