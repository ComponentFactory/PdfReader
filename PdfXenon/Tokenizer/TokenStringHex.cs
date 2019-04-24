using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenStringHex : TokenString
    {
        public TokenStringHex(long position, string rawString)
            : base(position, rawString)
        {
        }

        public override string ResolvedAsString
        {
            get
            {
                // Convert from hex to bytes
                byte[] raw = ResolvedAsBytes;

                // Check for the UTF16 Byte Order Mark (little endian or big endian versions)
                if ((raw.Length > 2) && (raw[0] == 0xFE) && (raw[1] == 0xFF))
                    return GetStringLiteralUTF16(raw, true);
                else if ((raw.Length > 2) && (raw[0] == 0xFF) && (raw[1] == 0xFE))
                    return GetStringLiteralUTF16(raw, false);
                else
                {
                    // Not unicode, so treat as ASCII
                    return Encoding.ASCII.GetString(raw);
                }
            }
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

        private string GetStringLiteralUTF16(byte[] raw, bool bigEndian)
        {
            int index = 0;
            int last = raw.Length - 1;

            if (bigEndian)
            {
                // Swap byte ordering
                byte temp;
                while (index < last)
                {
                    // Switch byte order of each character pair
                    temp = raw[index];
                    raw[index] = raw[index + 1];
                    raw[index + 1] = temp;
                    index += 2;
                }
            }

            return Encoding.Unicode.GetString(raw);
        }
    }
}
