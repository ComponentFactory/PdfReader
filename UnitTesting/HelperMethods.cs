using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTesting
{
    public class HelperMethods
    {
        public MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }

        public MemoryStream StringBytesToStream(string str1, byte[] bytes, string str2)
        {
            byte[] bytes1 = ASCIIEncoding.ASCII.GetBytes(str1);
            byte[] bytes2 = ASCIIEncoding.ASCII.GetBytes(str2);

            byte[] all = new byte[bytes1.Length + bytes.Length + bytes2.Length];
            Array.Copy(bytes1, 0, all, 0, bytes1.Length);
            Array.Copy(bytes, 0, all, bytes1.Length, bytes.Length);
            Array.Copy(bytes2, 0, all, bytes1.Length + bytes.Length, bytes2.Length);

            return new MemoryStream(all);
        }
    }
}
