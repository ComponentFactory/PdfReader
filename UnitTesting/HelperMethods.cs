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
    }
}
