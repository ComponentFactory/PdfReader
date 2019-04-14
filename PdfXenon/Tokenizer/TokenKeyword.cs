using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PdfXenon.Standard
{
    public class TokenKeyword : TokenObject
    {
        private static Dictionary<string, ParseKeyword> _lookup;

        static TokenKeyword()
        {
            _lookup = new Dictionary<string, ParseKeyword>();

            foreach (object val in Enum.GetValues(typeof(ParseKeyword)))
            {
                string name = Enum.GetName(typeof(ParseKeyword), val);
                string keyword = name;

                object[] attrs = typeof(ParseKeyword).GetMember(name)[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attrs != null) && (attrs.Length > 0))
                    keyword = ((DescriptionAttribute)attrs[0]).Description;

                _lookup.Add(keyword, (ParseKeyword)val);
            }
        }

        public TokenKeyword(long position, ParseKeyword keyword)
            : base(position)
        {
            Value = keyword;
        }

        public static TokenKeyword CheckKeywords(long position, string keyword)
        {
            if (_lookup.TryGetValue(keyword, out ParseKeyword pdfKeyword))
                return new TokenKeyword(position, pdfKeyword);

            return null;
        }

        public override string ToString()
        {
            return $"Keyword ({Position}): {Value}";
        }

        public ParseKeyword Value { get; private set; }
    }
}
