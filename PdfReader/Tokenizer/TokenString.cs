using System.Text;

namespace PdfReader
{
    public abstract class TokenString : TokenObject
    {
        public TokenString(string raw)
        {
            Raw = raw;
        }

        public string Raw { get; private set; }

        public abstract string Resolved { get; }
        public abstract byte[] ResolvedAsBytes { get; }
        public abstract string BytesToString(byte[] raw);

        protected string EncodedBytesToString(byte[] bytes)
        {
            // Check for the UTF16 Byte Order Mark (little endian or big endian versions)
            if ((bytes.Length > 2) && (bytes[0] == 0xFE) && (bytes[1] == 0xFF))
                return GetStringLiteralUTF16(bytes, true);
            else if ((bytes.Length > 2) && (bytes[0] == 0xFF) && (bytes[1] == 0xFE))
                return GetStringLiteralUTF16(bytes, false);
            else
            {
                // Not unicode, so treat as ASCII
                return Encoding.ASCII.GetString(bytes);
            }
        }

        private string GetStringLiteralUTF16(byte[] bytes, bool bigEndian)
        {
            int index = 0;
            int last = bytes.Length - 1;

            if (bigEndian)
            {
                // Swap byte ordering
                byte temp;
                while (index < last)
                {
                    // Switch byte order of each character pair
                    temp = bytes[index];
                    bytes[index] = bytes[index + 1];
                    bytes[index + 1] = temp;
                    index += 2;
                }
            }

            return Encoding.Unicode.GetString(bytes);
        }
    }
}
