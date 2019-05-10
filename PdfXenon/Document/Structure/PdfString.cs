using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfString : PdfObject
    {
        public PdfString(PdfObject parent, ParseString str)
            : base(parent, str)
        {
        }

        public override string ToString()
        {
            return Value;
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public ParseString ParseString { get => ParseObject as ParseString; }

        public string Value
        {
            get { return Decrypt.DecodeString(this); }
        }

        public byte[] ValueAsBytes
        {
            get { return Decrypt.DecodeStringAsBytes(this); }
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
                        throw new ApplicationException($"String '{Value}' cannot be converted to a date.");
                }
                catch
                {
                    throw new ApplicationException($"String '{Value}' cannot be converted to a date.");
                }
            }
        }
    }
}
