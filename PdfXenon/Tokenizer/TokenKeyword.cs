using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenKeyword : TokenBase
    {
        public static TokenKeyword True = new TokenKeyword("true");
        public static TokenKeyword False = new TokenKeyword("false");
        public static TokenKeyword Null = new TokenKeyword("null");

        private static Dictionary<string, TokenKeyword> _lookup;

        public static void Register(TokenKeyword token)
        {
            if (_lookup == null)
                _lookup = new Dictionary<string, TokenKeyword>();

            _lookup.Add(token.Keyword, token);
        }

        public static TokenKeyword CheckKeywords(string keyword)
        {
            _lookup.TryGetValue(keyword, out TokenKeyword ret);
            return ret;
        }

        public TokenKeyword(string keyword)
        {
            Keyword = keyword;
            Register(this);
        }

        public override string ToString()
        {
            return $"Keyword:{Keyword}";
        }

        public string Keyword { get; private set; }
    }
}
