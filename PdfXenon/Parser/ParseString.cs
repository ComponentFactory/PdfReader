using System;
using System.Globalization;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseString : ParseObject
    {
        public ParseString(TokenString token)
            : base(token.Position)
        {
            Token = token;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            string output = Value;
            sb.Append(output);
            return indent + output.Length;
        }

        public string Value
        {
            get { return Token.ResolvedAsString; }
        }

        public byte[] ValueAsBytes
        {
            get { return Token.ResolvedAsBytes; }
        }

        public DateTime ValueAsDateTime
        {
            get
            {
                try
                {
                    string str = Value;
                    if ((str != null) && (str.Length >= 4))
                    {
                        int index = 0;
                        int length = str.Length;

                        // The 'D:' prefix is optional
                        if ((str[index] == 'D') && (str[index + 1] == ':'))
                            index += 2;

                        // Year is mandatory, all the others are optional
                        int YYYY = int.Parse(str.Substring(index, 4));
                        int MM = (index + 4 < length) ? int.Parse(str.Substring(index + 4, 2)) : 1;
                        int DD = (index + 6 < length) ? int.Parse(str.Substring(index + 6, 2)) : 1;
                        int HH = (index + 7 < length) ? int.Parse(str.Substring(index + 8, 2)) : 0;
                        int mm = (index + 10 < length) ? int.Parse(str.Substring(index + 10, 2)) : 0;
                        int SS = (index + 12 < length) ? int.Parse(str.Substring(index + 12, 2)) : 0;
                        char O = (index + 14 < length) ? str[index + 14] : 'Z';
                        int OHH = (index + 15 < length) ? int.Parse(str.Substring(index + 15, 2)) : 0;
                        int OSS = (index + 18 < length) ? int.Parse(str.Substring(index + 18, 2)) : 0;
                        return new DateTime(YYYY, MM, DD, HH, mm, SS, DateTimeKind.Utc);
                    }
                    else
                        throw new ApplicationException($"String '{Value}' cannot be converted to a date at position {Position}.");
                }
                catch
                {
                    throw new ApplicationException($"String '{Value}' cannot be converted to a date at position {Position}.");
                }
            }
        }

        private TokenString Token { get; set; }
    }
}
