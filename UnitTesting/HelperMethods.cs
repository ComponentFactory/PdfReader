using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnitTesting
{
    public class HelperMethods
    {
        public MemoryStream BytesToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        public MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
