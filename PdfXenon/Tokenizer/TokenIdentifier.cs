using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class TokenIdentifier : TokenObject
    {
        private static ConcurrentDictionary<string, TokenIdentifier> _lookup = new ConcurrentDictionary<string, TokenIdentifier>();
        private static Func<string, TokenIdentifier, TokenIdentifier> _nullUpdate = (x, y) => y;

        public TokenIdentifier(string identifier)
        {
            Value = identifier;
        }

        public string Value { get; private set; }

        public static TokenIdentifier GetToken(string identifier)
        {
            if (!_lookup.TryGetValue(identifier, out TokenIdentifier tokenIdentifier))
            {
                tokenIdentifier = new TokenIdentifier(identifier);
                _lookup.AddOrUpdate(identifier, tokenIdentifier, _nullUpdate);
            }

            return tokenIdentifier;
        }
    }
}
