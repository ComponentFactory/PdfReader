using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class TokenReader
    {
        private Stream _stream;
        private byte[] _bytes;
        private int _start;
        private int _end;

        public TokenReader(Stream stream)
        {
            _stream = stream;
            Position = stream.Position;
            _bytes = new byte[1024];
        }

        public long Position { get; private set; }

        public byte[] GetBytes(int length)
        {
            // Make sure we have some data to process
            if ((_start == _end) && (ReadBytes() == 0))
                return null;

            // Skip over any line feed at end of preceding line
            if (_bytes[_start] == '\r')
                _start++;

            byte[] ret = new byte[length];
            int index = 0;

            // Copy any remaining bytes in the buffer
            if (_start < _end)
            {
                int copy = Math.Min(length, (_end - _start));
                Array.Copy(_bytes, _start, ret, 0, copy);
                index += copy;
                _start += copy;
            }

            // Read remaining bytes directly from the stream
            if (index < length)
            {
                int copied = _stream.Read(ret, index, length - index);
                if (copied < (length - index))
                    return null;
            }

            return ret;
        }

        public string ReadLine()
        {
            // If there is no more content to return, then return null
            if ((_start == _end) && (ReadBytes() == 0))
                return null;

            StringBuilder sb = new StringBuilder();

            do
            {
                int index = _start;

                do
                {
                    byte c = _bytes[index];

                    // Reached an end of line marker?
                    if ((c == '\r') || (c == '\n'))
                    {
                        // Append the unprocessed characters before the end of line marker
                        sb.Append(Encoding.ASCII.GetChars(_bytes, _start, index - _start));

                        // Processing continues after the first end of line marker
                        _start = index + 1;
                        Position++;

                        // Check if newline has a linefeed afterwards
                        if ((c == '\r') && ((_start < _end) || (ReadBytes() > 0)))
                        {
                            if (_bytes[_start] == '\n')
                            {
                                // Skip over the linefeed
                                _start++;
                                Position++;
                            }
                        }

                        return sb.ToString();
                    }

                    index++;
                    Position++;

                } while (index < _end);

                // Append the unprocessed characters
                sb.Append(Encoding.ASCII.GetChars(_bytes, _start, index - _start));

            } while (ReadBytes() > 0);

            return sb.ToString();
        }

        private int ReadBytes()
        {
            // Read in a buffer of ASCII characters
            _start = 0;
            _end = _stream.Read(_bytes, 0, _bytes.Length);
            return _end;
        }
    }
}
