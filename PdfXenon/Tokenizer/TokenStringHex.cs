using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenStringHex : TokenString
    {
        public TokenStringHex(string raw)
            : base(raw)
        {
        }

        public override string Resolved
        {
            get { return BytesToString(ResolvedAsBytes); }
        }

        public override byte[] ResolvedAsBytes
        {
            get
            {
                // Remove all whitespace from the hex string
                string[] sections = Raw.Split(new char[] { '\0', '\t', '\n', '\f', '\r', ' ' });
                string hex = string.Join(string.Empty, sections);

                // If a missing character from last hex pair, then default to 0, as per the spec
                if ((hex.Length % 2) == 1)
                    hex += "0";

                // Convert from hex to bytes
                byte[] raw = new byte[hex.Length / 2];
                for (int i = 0; i < raw.Length; i++)
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);

                return raw;
            }
        }

        public override string BytesToString(byte[] bytes)
        {
            return EncodedBytesToString(bytes);
        }
    }
}
