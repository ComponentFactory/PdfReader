using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenStringHex : TokenString
    {
        private string _actual;

        public TokenStringHex(long position, string rawString)
            : base(position, rawString)
        {
        }

        public override string Resolved
        {
            get
            {
                if (_actual == null)
                {
                    // Remove all whitespace from the hex string
                    string[] sections = Raw.Split(new char[] { '\0', '\t', '\n', '\f', '\r', ' ' });
                    string hex = string.Join(string.Empty, sections);

                    // If a missing character from last hex pair, then default to 0, as per the spec
                    if ((hex.Length % 2) == 1)
                        hex += "0";

                    // Convert from hex to actual characters
                    byte[] raw = new byte[hex.Length / 2];
                    for (int i = 0; i < raw.Length; i++)
                        raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);

                    _actual = Encoding.ASCII.GetString(raw);
                }

                return _actual;
            }
        }
    }
}
