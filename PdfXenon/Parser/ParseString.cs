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
            get { return Token.Resolved; }
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

                        // Ignore any UTC adjustment because we want dates in local time
                        string dt = string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2}", YYYY, MM, DD, HH, mm, SS);
                        return DateTime.ParseExact(dt, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
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
