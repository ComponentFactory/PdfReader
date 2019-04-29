using System.Collections.Generic;

namespace PdfXenon.Standard
{
    public class TokenIdentifier : TokenObject
    {
        private static Dictionary<string, TokenIdentifier> _lookup = new Dictionary<string, TokenIdentifier>();

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
                _lookup.Add(identifier, tokenIdentifier);
            }

            return tokenIdentifier;
        }
    }
}
