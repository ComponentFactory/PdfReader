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
            byte[] bytes = _streamBytes;

            if (HasFilter)
            {
                // Get the filtering as an array to be applied in order (if a single filter then convert from Name to an Array of one entry)
                ParseDictEntry entry = _dictionary["Filter"];
                ParseArray filters = entry.Object as ParseArray;
                if ((filters == null) && (entry.Object is ParseName))
                    filters = new ParseArray(entry.Object.Position, new List<ParseObject>() { entry.Object });

                foreach (ParseName filter in filters.Objects)
                {
                    switch (filter.Value)
                    {
                        case "FlateDecode":
                            bytes = FlateDecode(bytes);
                            break;
                        // TODO
                        //case "DCTDecode":
                        //    bytes = DCTDecode(bytes);
                        //    break;
                        default:
                            throw new ApplicationException($"Cannot process unrecognized stream filter '{filter.Value}' at {Position}.");
                    }
                }
            }

            return bytes;
        }

        private byte[] FlateDecode(byte[] bytes)
        {
            using (MemoryStream inputStream = new MemoryStream(bytes))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (DeflateStream decodeStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                    {
                        // Skip the two byte zlib header
                        inputStream.Position = 2;
                        decodeStream.CopyTo(outputStream);
                        bytes = outputStream.GetBuffer();
                    }
                }
            }

            // TODO
            if (_dictionary.ContainsName("Predictor"))
                throw new ApplicationException($"Cannot process FlatDecode predictors at {Position}.");

            return bytes;
        }

        private byte[] DCTDecode(byte[] bytes)
        {
            // TODO
            return bytes;
        }
    }
}
