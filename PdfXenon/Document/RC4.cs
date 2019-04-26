using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfXenon.Standard
{
    public static class RC4
    {
        public static byte[] Transform(byte[] key, byte[] data)
        {
            byte[] s = EncryptInitalize(key);
            byte[] ret = new byte[data.Length];

            int i = 0;
            int j = 0;
            for(int k = 0; k < data.Length; k++)
            {
                i = (i + 1) & 255;
                j = (j + s[i]) & 255;
                Swap(s, i, j);

                ret[k] = (byte)(data[k] ^ s[(s[i] + s[j]) & 255]);
            }

            return ret;
        }

        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = new byte[256];
            for (int i = 0; i < s.Length; i++)
                s[i] = (byte)i;

            int j = 0;
            for (int i = 0; i < 256; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 255;
                Swap(s, i, j);
            }

            return s;
        }

        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];
            s[i] = s[j];
            s[j] = c;
        }
    }
}
