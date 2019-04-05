using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfStream : PdfObject
    {
        private string _stringContent;
        private byte[] _byteContent;

        public PdfStream(PdfDictionary dictionary, byte[] bytes)
        {
            Dictionary = dictionary;
            Bytes = bytes;
        }

        public override long Position { get => Dictionary.Position; }

        public string StringContent
        {
            get
            {
                if (_stringContent == null)
                    _stringContent = Encoding.ASCII.GetString(DecodeContent());

                return _stringContent;
            }
        }

        public byte[] ByteContent
        {
            get
            {
                if (_byteContent == null)
                    _byteContent = DecodeContent();

                return _byteContent;
            }
        }

        private PdfDictionary Dictionary { get; set; }
        private byte[] Bytes { get; set; }

        private byte[] DecodeContent()
        {
            if (Dictionary.ContainsKey("Filter"))
            {
                PdfDictEntry entry = Dictionary["Filter"];

                // Get the array of filers that need applying (if a single filter then convert to array of one entry)
                PdfArray filters = entry.Object as PdfArray;
                if (filters == null)
                    filters = new PdfArray(entry.Object.Position, new List<PdfObject> { entry.Object });

                // Apply each filter in turn
                byte[] bytes = Bytes;
                foreach (PdfObject filter in filters.Objects)
                {
                    PdfName name = filter as PdfName;
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
