using System;
using System.IO;

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
                Position += copy;
            }

            // Read remaining bytes directly from the stream
            if (index < length)
            {
                int copied = _stream.Read(ret, index, length - index);
                Position += copied;

                if (copied < (length - index))
                    return null;
            }

            return ret;
        }

        public byte[] ReadLine()
        {
            // If there is no more content to return, then return null
            if ((_start == _end) && (ReadBytes() == 0))
                return null;

            byte[] ret = null;

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
                        ret = AppendBytes(_bytes, _start, index - _start, ret);

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

                        return ret;
                    }

                    index++;
                    Position++;

                } while (index < _end);

                // Append the unprocessed characters
                ret = AppendBytes(_bytes, _start, index - _start, ret);

            } while (ReadBytes() > 0);

            return ret;
        }

        private int ReadBytes()
        {
            // Read in a buffer of ASCII characters
            _start = 0;
            _end = _stream.Read(_bytes, 0, _bytes.Length);
            return _end;
        }

        private byte[] AppendBytes(byte[] bytes, int start, int length, byte[] existing)
        {
            if (existing == null)
            {
                byte[] ret = new byte[length];
                Array.Copy(bytes, start, ret, 0, length);
                return ret;
            }
            else
            {
                byte[] ret = new byte[existing.Length + length];
                Array.Copy(existing, 0, ret, 0, existing.Length);
                Array.Copy(bytes, start, ret, existing.Length, length);
                return ret;
            }
        }

    }
}
