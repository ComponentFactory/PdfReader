using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PdfXenon.Standard
{
    public class TokenKeyword : TokenObject
    {
        private static Dictionary<string, TokenKeyword> _lookup;

        static TokenKeyword()
        {
            _lookup = new Dictionary<string, TokenKeyword>();

            foreach (object val in Enum.GetValues(typeof(ParseKeyword)))
            {
                string name = Enum.GetName(typeof(ParseKeyword), val);
                string keyword = name;

                object[] attrs = typeof(ParseKeyword).GetMember(name)[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attrs != null) && (attrs.Length > 0))
                    keyword = ((DescriptionAttribute)attrs[0]).Description;

                _lookup.Add(keyword, new TokenKeyword((ParseKeyword)val));
            }
        }

        public TokenKeyword(ParseKeyword keyword)
        {
            Value = keyword;
        }

        public ParseKeyword Value { get; private set; }

        public static TokenKeyword GetToken(string keyword)
        {
            _lookup.TryGetValue(keyword, out TokenKeyword tokenKeyword);
            return tokenKeyword;
        }
    }
}
