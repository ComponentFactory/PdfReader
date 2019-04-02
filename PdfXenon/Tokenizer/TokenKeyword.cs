using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenKeyword : TokenBase
    {
        private static Dictionary<string, PdfKeyword> _lookup;

        static TokenKeyword()
        {
            _lookup = new Dictionary<string, PdfKeyword>();

            foreach (object val in Enum.GetValues(typeof(PdfKeyword)))
            {
                string name = Enum.GetName(typeof(PdfKeyword), val);
                string keyword = name;

                object[] attrs = typeof(PdfKeyword).GetMember(name)[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((attrs != null) && (attrs.Length > 0))
                    keyword = ((DescriptionAttribute)attrs[0]).Description;

                _lookup.Add(keyword, (PdfKeyword)val);
            }
        }

        public static TokenKeyword CheckKeywords(long position, string keyword)
        {
            if (_lookup.TryGetValue(keyword, out PdfKeyword pdfKeyword))
                return new TokenKeyword(position, pdfKeyword);

            return null;
        }

        public TokenKeyword(long position, PdfKeyword keyword)
            : base(position)
        {
            Keyword = keyword;
        }

        public override string ToString()
        {
            return $"Keyword: {Keyword}, Pos: {Position}";
        }

        public PdfKeyword Keyword { get; private set; }
    }
}
