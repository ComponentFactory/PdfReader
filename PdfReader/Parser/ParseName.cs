using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;

namespace PdfReader
{
    public class ParseName : ParseObjectBase
    {
        private static ConcurrentDictionary<string, ParseName> _lookup = new ConcurrentDictionary<string, ParseName>();
        private static Func<string, ParseName, ParseName> _nullUpdate = (x, y) => y;

        public ParseName(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public static ParseName GetParse(string name)
        {
            if (!_lookup.TryGetValue(name, out ParseName parseName))
            {
                parseName = new ParseName(name);
                _lookup.AddOrUpdate(name, parseName, _nullUpdate);
            }

            return parseName;
        }
    }
}
