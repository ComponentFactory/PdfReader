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
        private long _position;

        public TokenReader(Stream stream)
        {
            _stream = stream;
            _position = stream.Position;
            _bytes = new byte[1024];
        }

        public long Position { get => _position; }

        public byte[] GetBytes(int length)
        {
            // Make sure we have some data to process
            if ((_start == _end) && (ReadBytes(false) == 0))
                return null;

            byte[] ret = new byte[length];
            int index = 0;

            // Copy any remaining bytes in the buffer
            if (_start < _end)
            {
                int copy = Math.Min(length, (_end - _start));
                Array.Copy(_bytes, _start, ret, 0, copy);
                index += copy;
                _start += copy;
                _position += copy;
            }

            // Read remaining bytes directly from the stream
            if (index < length)
            {
                int copied = _stream.Read(ret, index, length - index);
                _position += copied;

                if (copied < (length - index))
                    return null;
            }

            return ret;
        }

        public TokenByteSplice ReadLine()
        {
            // If there is no more content to return, then return null
            if ((_start == _end) && (ReadBytes(false) == 0))
                return new TokenByteSplice();

            TokenByteSplice ret = new TokenByteSplice();

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
                        AppendBytes(_bytes, _start, index - _start, ref ret);

                        // Processing continues after the first end of line marker
                        _start = index + 1;
                        _position++;

                        // Check if newline has a linefeed afterwards
                        if ((c == '\r') && ((_start < _end) || (ReadBytes(true) > 0)))
                        {
                            if (_bytes[_start] == '\n')
                            {
                                // Skip over the linefeed
                                _start++;
                                _position++;
                            }
                        }

                        return ret;
                    }

                    index++;
                    _position++;

                } while (index < _end);

                // Append the unprocessed characters
                AppendBytes(_bytes, _start, index - _start, ref ret);

            } while (ReadBytes(true) > 0);

            return ret;
        }

        public void Reset()
        {
            _start = 0;
            _end = 0;
            _position = _stream.Position;
        }

        private int ReadBytes(bool newBuffer)
        {
            // Read in a buffer of ASCII characters
            _start = 0;

            if (newBuffer)
                _bytes = new byte[1024];

            _end = _stream.Read(_bytes, 0, _bytes.Length);
            return _end;
        }

        private void AppendBytes(byte[] bytes, int start, int length, ref TokenByteSplice existing)
        {
            if (existing.Bytes == null)
            {
                existing.Bytes = bytes;
                existing.Start = start;
                existing.Length = length;
            }
            else
            {
                byte[] ret = new byte[existing.Length + length];
                Array.Copy(existing.Bytes, existing.Start, ret, 0, existing.Length);
                Array.Copy(bytes, start, ret, existing.Length, length);

                existing.Bytes = ret;
                existing.Start = 0;
                existing.Length = ret.Length;
            }
        }
    }
}
