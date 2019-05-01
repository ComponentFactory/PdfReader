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

        public ParseStream(ParseDictionary dictionary, byte[] streamBytes)
        {
            _dictionary = dictionary;
            StreamBytes = streamBytes;
        }

        public override int Output(StringBuilder sb, int indent)
        {
            _dictionary.Output(sb, indent);

            string blank = new string(' ', indent);
            sb.Append("\n");
            sb.Append(blank);
            sb.Append("stream\n");

            sb.Append(blank);
            sb.Append(Value);

            sb.Append("\n");
            sb.Append(blank);
            sb.Append("endstream");

            return indent;
        }

        public byte[] StreamBytes { get; private set; }

        public bool HasFilter
        {
            get { return _dictionary.ContainsName("Filter"); }
        }

        public string Value
        {
            get { return Encoding.ASCII.GetString(DecodeBytes(StreamBytes)); }
        }

        public byte[] ValueAsBytes
        {
            get { return DecodeBytes(StreamBytes); }
        }

        public byte[] DecodeBytes(byte[] bytes)
        {
            if (HasFilter)
            {
                // Get the filtering as an array to be applied in order (if a single filter then convert from Name to an Array of one entry)
                ParseObject obj = _dictionary["Filter"];
                ParseArray filters = obj as ParseArray;
                if ((filters == null) && (obj is ParseName))
                    filters = new ParseArray(new List<ParseObject>() { obj });

                foreach (ParseName filter in filters.Objects)
                {
                    switch (filter.Value)
                    {
                        case "FlateDecode":
                            bytes = FlateDecode(bytes);
                            break;
                        default:
                            throw new ApplicationException($"Cannot process unrecognized stream filter '{filter.Value}'.");
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
                        // Skip the zlib 2 byte header
                        inputStream.Position = 2;
                        decodeStream.CopyTo(outputStream);
                        bytes = outputStream.GetBuffer();
                    }
                }
            }

            // TODO
            if (_dictionary.ContainsName("Predictor"))
                throw new ApplicationException($"Cannot process FlatDecode predictors.");

            return bytes;
        }
    }
}
