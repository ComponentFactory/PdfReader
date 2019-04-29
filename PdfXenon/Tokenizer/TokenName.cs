using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class TokenName : TokenObject
    {
        private static ConcurrentDictionary<string, TokenName> _lookup = new ConcurrentDictionary<string, TokenName>();
        private static Func<string, TokenName, TokenName> _nullUpdate = (x, y) => y;

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
                _lookup.AddOrUpdate(name, tokenName, _nullUpdate);
            }

            return tokenName;
        }
    }
}
