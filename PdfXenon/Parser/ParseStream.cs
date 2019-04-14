using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseStream : ParseObject
    {
        private ParseDictionary _dictionary;
        private byte[] _streamBytes;
        private string _stringContent;

        public ParseStream(ParseDictionary dictionary, byte[] streamBytes)
            : base(dictionary.Position)
        {
            _dictionary = dictionary;
            _streamBytes = streamBytes;
        }

        public override string ToString()
        {
            if (HasFilter)
                return $"ParseStream ({Position}): Filtered Len:{_streamBytes.Length}";
            else
                return $"ParseStream ({Position}): {ContentAsString}";
        }

        public bool HasFilter
        {
            get { return _dictionary.ContainsName("Filter"); }
        }

        public string ContentAsString
        {
            get
            {
                if (_stringContent == null)
                {
                    if (_streamBytes == null)
                        return string.Empty;
                    else
                        _stringContent = Encoding.ASCII.GetString(_streamBytes);
                }

                return _stringContent;
            }
        }
    }
}
