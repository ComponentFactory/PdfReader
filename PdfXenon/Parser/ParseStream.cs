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
        private byte[] _byteContent;
        private string _stringContent;

        public ParseStream(ParseDictionary dictionary, byte[] streamBytes)
            : base(dictionary.Position)
        {
            _dictionary = dictionary;
            _streamBytes = streamBytes;
        }

        public override string ToString()
        {
            return $"ParseStream ({Position}): {ContentAsString}";
        }

        public bool HasFilter
        {
            get { return _dictionary.ContainsName("Filter"); }
        }

        public byte[] ContentAsBytes
        {
            get
            {
                if (_byteContent == null)
                {
                    if (_streamBytes == null)
                        return null;
                    else
                        _byteContent = DecodedBytes();
                }

                return _byteContent;
            }
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
                        _stringContent = Encoding.ASCII.GetString(DecodedBytes());
                }

                return _stringContent;
            }
        }

        private byte[] DecodedBytes()
        {
            if (!HasFilter)
                return _streamBytes;

            using (MemoryStream inputStream = new MemoryStream(_streamBytes))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (DeflateStream decodeStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                    {
                        // Skip the two byte zlib header
                        inputStream.Position = 2;
                        decodeStream.CopyTo(outputStream);
                        return outputStream.GetBuffer();
                    }
                }
            }
        }
    }
}
