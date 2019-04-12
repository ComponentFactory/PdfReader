using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PdfXenon.Standard
{
    public class ParseStream : ParseObject
    {
        private byte[] _byteContent;
        private string _stringContent;

        public ParseStream(ParseDictionary dictionary, byte[] bytes)
        {
            Dictionary = dictionary;
            Bytes = bytes;
        }

        public override string ToString()
        {
            return $"ParseStream ({Position}): Len:{Bytes.Length}";
        }

        public override long Position { get => Dictionary.Position; }

        public string ContentAsString
        {
            get
            {
                if (_stringContent == null)
                    _stringContent = Encoding.ASCII.GetString(DecodeContent());

                return _stringContent;
            }
        }

        public byte[] ContentAsBytes
        {
            get
            {
                if (_byteContent == null)
                    _byteContent = DecodeContent();

                return _byteContent;
            }
        }

        private ParseDictionary Dictionary { get; set; }
        private byte[] Bytes { get; set; }

        private byte[] DecodeContent()
        {
            if (Dictionary.ContainsName("Filter"))
            {
                ParseDictEntry entry = Dictionary["Filter"];

                // Get the array of filers that need applying (if a single filter then convert to array of one entry)
                ParseArray filters = entry.Object as ParseArray;
                if (filters == null)
                    filters = new ParseArray(entry.Object.Position, new List<ParseObject> { entry.Object });

                // Apply each filter in turn
                byte[] bytes = Bytes;
                foreach (ParseObject filter in filters.Objects)
                {
                    ParseName name = filter as ParseName;
                    if (name == null)
                        throw new ApplicationException($"Stream filter is type {name.GetType().Name} instead of a name, at position {name.Position}.");

                    switch (name.Name)
                    {
                        case "FlateDecode":
                            bytes = FlateDecode(bytes);
                            break;
                        default:
                            throw new ApplicationException($"Stream filter {name.GetType().Name} is unrecognized , at position {name.Position}.");
                    }
                }

                return bytes;
            }
            else
                return Bytes;
        }

        private byte[] FlateDecode(byte[] bytes)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress, true))
                {
                    // Skip the two byte header of zlib
                    deflateStream.Write(bytes, 2, bytes.Length - 2);
                }

                return outputStream.ToArray();
            }
        }
    }
}
