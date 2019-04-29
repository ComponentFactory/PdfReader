using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Tokenizer
    {
        private const int EOF_SCAN_LENGTH = 1024;

        // Lookup arrays are fast and small because the source is ASCII characters, so limimted to a possible 256 values
        private static bool[] _lookupWhitespace;
        private static bool[] _lookupDelimiter;
        private static bool[] _lookupDelimiterWhitespace;
        private static int[]  _lookupHexToDecimal;
        private static bool[] _lookupHexadecimal;
        private static bool[] _lookupHexadecimalWhitespace;
        private static bool[] _lookupIsNumeric;
        private static bool[] _lookupIsNumericStart;
        private static bool[] _lookupKeyword;
        private static Dictionary<string, string> _uniqueStrings = new Dictionary<string, string>();

        private static readonly byte[] _whitespace = new byte[] { 0, 9, 10, 12, 13, 32 }; 
        //                                                       \0  \t \n  \f  \r  (SPACE)

        private static readonly byte[] _delimiter = new byte[] { 40, 41, 60, 62, 91, 93, 123, 125, 47, 37 }; 
        //                                                       (   )   <   >   [   ]   {    }    /   %

        private static readonly byte[] _hexadecimal = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70, 97, 98, 99, 100, 101, 102 };
        //                                                         0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F   a   b   c   d    e    f

        private static readonly byte[] _isNumeric = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57 };
        //                                                       0   1   2   3   4   5   6   7   8   9

        private static readonly byte[] _isNumericStart = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 43, 45, 46 };
        //                                                            0   1   2   3   4   5   6   7   8   9   +   -   .

        private static readonly byte[] EOF_COMMENT = new byte[] { 70, 79, 69, 37, 37 }; 
        //                                                        F   O   E   %   %

        private int _index;
        private int _length;
        private long _position;
        private byte[] _line;
        private bool _disposed;
        private TokenReader _reader;
        private Stack<TokenObject> _stack = new Stack<TokenObject>();

        static Tokenizer()
        {
            _lookupWhitespace = new bool[256];
            foreach (byte code in _whitespace)
                _lookupWhitespace[code] = true;

            _lookupDelimiter = new bool[256];
            foreach (byte code in _delimiter)
                _lookupDelimiter[code] = true;

            _lookupHexadecimal = new bool[256];
            foreach (byte code in _hexadecimal)
                _lookupHexadecimal[code] = true;

            _lookupIsNumeric = new bool[256];
            foreach (byte code in _isNumeric)
                _lookupIsNumeric[code] = true;

            _lookupIsNumericStart = new bool[256];
            foreach (byte code in _isNumericStart)
                _lookupIsNumericStart[code] = true;

            _lookupHexadecimalWhitespace = new bool[256];
            foreach (byte code in _whitespace)
                _lookupHexadecimalWhitespace[code] = true;
            foreach (byte code in _hexadecimal)
                _lookupHexadecimalWhitespace[code] = true;

            _lookupDelimiterWhitespace = new bool[256];
            foreach (byte code in _whitespace)
                _lookupDelimiterWhitespace[code] = true;
            foreach (byte code in _delimiter)
                _lookupDelimiterWhitespace[code] = true;

            _lookupHexToDecimal = new int[256];
            for (int i = 0; i < 10; i++)
                _lookupHexToDecimal[48 + i] = i;    // '0' + i
            for (int i = 0; i < 6; i++)
            {
                _lookupHexToDecimal[65 + i] = i;    // 'a' + i
                _lookupHexToDecimal[97 + i] = i;    // 'A' + i
            }

            _lookupKeyword = new bool[256];
            for (int i = 65; i <= 90; i++)  // 'a' -> 'z'
                _lookupKeyword[i] = true;
            for (int i = 97; i <= 122; i++) // 'A' -> 'Z'
                _lookupKeyword[i] = true;
        }

        public Tokenizer(Stream stream)
        {
            // Must have an actual stream reference
            Stream = stream ?? throw new ArgumentNullException("stream");

            // Stream is no use if we cannot read from it!
            if (!stream.CanRead)
                throw new ApplicationException("Cannot read from stream.");

            // Stream must be able to be randomly positioned
            if (!stream.CanSeek)
                throw new ApplicationException("Cannot seek within stream.");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public bool AllowIdentifiers { get; set; } = false;
        public bool IgnoreComments { get; set; } = true;

        public long Position
        {
            get
            {
                // The Reader class keeps track of the actual cursor position when buffering data
                if (Reader != null)
                    return Reader.Position;
                else
                    return Stream.Position;
            }

            set
            {
                // Clear any cached bytes
                _reader = null;
                _line = null;

                // Move stream to desired offset
                Stream.Position = value;
            }
        }

        public void PushToken(TokenObject token)
        {
            _stack.Push(token);
        }

        public TokenObject GetToken()
        {
            TokenObject t = null;
            if (_stack.Count > 0)
                t = _stack.Pop();
            else
                t = GetAnyToken();

            if (IgnoreComments)
            {
                while (t is TokenComment)
                {
                    if (_stack.Count > 0)
                        t = _stack.Pop();
                    else
                        t = GetAnyToken();
                }
            }

            return t;
        }

        public bool GotoNextLine()
        {
            _position = Reader.Position;
            _line = Reader.ReadLine();

            if (_line != null)
            {
                _length = _line.Length;
                _index = 0;
                return true;
            }
            else
                return false;
        }

        public byte[] GetBytes(int length)
        {
            return Reader.GetBytes(length);
        }

        public TokenObject GetXRefEntry(int id)
        {
            // Ignore zero or more whitespace characters
            SkipWhitespace();

            if (_length < 18)
                return new TokenError(_position, $"Cross-reference entry data is {_length} bytes instead of the expected 18.");

            _index = _length;

            return new TokenXRefEntry(id,
                                      ConvertDecimalToInteger(11, 5),
                                      ConvertDecimalToInteger(0, 10),
                                      (_line[17] == 110)); // 'n'
        }

        public long GetXRefOffset()
        {
            // We expect the stream to be at least a certain minimal size
            if (Stream.Length < EOF_SCAN_LENGTH)
                throw new ApplicationException($"Stream must be at least {EOF_SCAN_LENGTH} bytes.");

            // Load bytes from the end, enough to discover the location of the 'xref' section
            byte[] bytes = new byte[EOF_SCAN_LENGTH];
            Stream.Position = Stream.Length - bytes.Length;
            if (Stream.Read(bytes, 0, bytes.Length) != bytes.Length)
                throw new ApplicationException($"Failed to read in last {EOF_SCAN_LENGTH} bytes of stream.");

            // Start scanning backwards from the end
            int index = bytes.Length - 1;

            // Find the %%EOF comment
            int match = 0;
            while(index > 0)
            {
                if (EOF_COMMENT[match] == bytes[index])
                {
                    match++;
                    if (match == EOF_COMMENT.Length)
                    {
                        index--;
                        break;
                    }
                }
                else if (EOF_COMMENT[0] == bytes[index])
                    match = 1;
                else
                    match = 0;

                index--;
            }

            if (index == 0)
                throw new ApplicationException($"Could not find %%EOF comment at end of the stream.");

            // Skip any whitespace
            while (_lookupWhitespace[bytes[index]])
                index--;

            if (index == 0)
                throw new ApplicationException($"Could not find offset of the cross-reference table.");

            int end = index;
            while (_lookupIsNumericStart[bytes[end]])
                end--;

            if (index == 0)
                throw new ApplicationException($"Could not find offset of the cross-reference table.");

            return ConvertDecimalToLong(bytes, end + 1, index - end);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _reader = null;
                    Stream.Dispose();
                    Stream = null;
                }

                _disposed = true;
            }
        }

        private TokenReader Reader
        {
            get
            {
                if (_reader == null)
                    _reader = new TokenReader(Stream);

                return _reader;
            }
        }

        private int ConvertDecimalToInteger(int start, int length)
        {
            int ret = 0;
            int index = start;

            for (int i = 0; i < length; i++)
            {
                ret *= 10;
                ret += _line[index++] - 48; // '0'
            }

            return ret;
        }

        private long ConvertDecimalToLong(byte[] bytes, int start, int length)
        {
            long ret = 0;
            int index = start;

            for (int i = 0; i < length; i++)
            {
                ret *= 10;
                ret += bytes[index++] - 48; // '0'
            }

            return ret;
        }

        private void SkipWhitespace()
        {
            while (true)
            {
                // Do we need to fetch the next line of characters?
                if ((_line == null) || (_index == _length))
                {
                    _position = Reader.Position;
                    _line = Reader.ReadLine();

                    if (_line != null)
                    {
                        _length = _line.Length;
                        _index = 0;
                    }
                    else
                    {
                        // No more lines, finished
                        break;
                    }
                }

                // Skip all whitespace characters
                while (_index < _length)
                {
                    if (_lookupWhitespace[_line[_index]])
                        _index++;
                    else
                        return;
                }
            }
        }

        private TokenObject GetAnyToken()
        {
            // Ignore zero or more whitespace characters
            SkipWhitespace();

            // Have we run out of content?
            if ((_line == null) || (_index == _length))
                return TokenObject.Empty;
            else
            {
                long position = _position + _index;

                if (_lookupIsNumericStart[_line[_index]])
                    return GetNumber();
                else if (!_lookupDelimiter[_line[_index]])
                    return GetKeywordOrIdentifier();
                else
                {
                    switch (_line[_index])
                    {
                        case 37: // '%'
                            return GetComment();
                        case 47: // '/'
                            return GetName();
                        case 60: // '<'
                            return GetDictionaryOpenOrHexString();
                        case 62: // '>'
                            return GetDictionaryClose();
                        case 40://  '('
                            return GetStringLiteral();
                        case 91: // '['
                            _index++;
                            return TokenObject.ArrayOpen;
                        case 93: // ']'
                            _index++;
                            return TokenObject.ArrayClose;
                    }

                    // Found invalid character for this position
                    return new TokenError(position, $"Cannot parse '{_line[_index]}' as a delimiter or regular character.");
                }
            }
        }

        private TokenObject GetNumber()
        {
            long position = _position + _index;

            bool positive = true;
            int start = _index;
            byte current = _line[_index++];

            // Check for sign
            if (current == 43) // '+'
            {
                if (_index < _length)
                    current = _line[_index++];
                else
                    return new TokenError(position, $"Cannot parse number because unexpected end-of-line encountered after '+'.");
            }
            else if (current == 45) // '-'
            {
                positive = false;

                if (_index < _length)
                    current = _line[_index++];
                else
                    return new TokenError(position, $"Cannot parse number because unexpected end-of-line encountered after '-'.");
            }

            // Convert whole number part
            int whole = 0;
            while(true)
            {
                if (_lookupIsNumeric[current])
                {
                    whole = (whole * 10) + (current - 48);

                    if (_index < _length)
                        current = _line[_index++];
                    else
                        return new TokenInteger(positive ? whole : -whole);
                }
                else
                    break;
            }

            // Is there is no fractional part then it must be an integer
            if (current != 46)
            {
                _index--;
                return new TokenInteger(positive ? whole : -whole);
            }

            if (_index < _length)
                current = _line[_index++];
            else
                return new TokenReal(positive ? whole : -whole);

            // Find end of the fractional part
            while ((_index < _length) && _lookupIsNumeric[_line[_index]])
                _index++;

            string text = Encoding.ASCII.GetString(_line, start, _index - start);
            if (float.TryParse(text, out float convert))
                return new TokenReal(convert);

            // String is not a recognized number format
            return new TokenError(position, $"Cannot parse text as a real number.");
        }

        private TokenObject GetKeywordOrIdentifier()
        {
            long position = _position + _index;

            // Scan looking for the end of the keyword characters
            int key = _index;
            while ((key < _length) && _lookupKeyword[_line[key]])
                key++;

            string text = Encoding.ASCII.GetString(_line, _index, key - _index);
            TokenKeyword token = TokenKeyword.GetToken(text);
            if (token != null)
            {
                _index = key;
                return token;
            }

            if (AllowIdentifiers)
            {
                // Scan looking for the end of the identifier
                key = _index;
                while ((key < _length) && !_lookupDelimiterWhitespace[_line[key]])
                    key++;

                text = Encoding.ASCII.GetString(_line, _index, key - _index);
                _index = key;

                return new TokenIdentifier(UniqueString(text));
            }
            else
                return new TokenError(position, $"Cannot parse '{text}' as a keyword.");
        }

        private TokenObject GetComment()
        {
            long position = _position + _index;

            // Everything till end of the line is the comment
            string comment = Encoding.ASCII.GetString(_line, _index, _length - _index);

            // Continue processing at start of the next line
            _line = null;

            return new TokenComment(comment);
        }


        private TokenObject GetName()
        {
            long position = _position + _index;

            // Find the run of regular characters
            int end = _index + 1;
            while ((end < _length) && !_lookupDelimiterWhitespace[_line[end]])
                end++;

            string name = Encoding.ASCII.GetString(_line, _index + 1, end - _index - 1);
            _index = end;

            // Convert any escape sequences
            while (true)
            {
                int escape = name.IndexOf('#');
                if (escape < 0)
                    break;

                // Check there are two digits after it
                if ((escape > (name.Length - 3)) || !_lookupHexadecimal[(byte)name[escape + 1]] || !_lookupHexadecimal[(byte)name[escape + 2]])
                    return new TokenError(position + escape, $"Escaped character inside name is not followed by two hex digits.");

                char val = (char)(_lookupHexToDecimal[(byte)name[escape + 1]] * 16 + _lookupHexToDecimal[(byte)name[escape + 2]]);
                name = name.Replace(name.Substring(escape, 3), $"{val}");
            }

            return TokenName.GetToken(UniqueString(name));
        }

        private TokenObject GetDictionaryOpenOrHexString()
        {
            long position = _position + _index;

            _index++;
            if (_index >= _length)
                return new TokenError(position, $"Unexpected end of line after '<'.");

            // Is the next character another '<'
            if (_line[_index] == '<')
            {
                _index++;
                return TokenObject.DictionaryOpen;
            }
            else
            {
                // Find the run of hexadecimal characters and whitespace
                int end = _index;
                while ((end < _length) && _lookupHexadecimalWhitespace[_line[end]])
                    end++;

                if (end == _length)
                    return new TokenError(position, $"Missing '>' at end of hexadecimal string.");

                if (_line[end] != '>')
                    return new TokenError(position, $"Invalid character '{_line[end]}' found in hexadecimal string.");

                string str = Encoding.ASCII.GetString(_line, _index, end - _index);
                _index = end + 1;

                return new TokenStringHex(str);
            }
        }

        private TokenObject GetDictionaryClose()
        {
            long position = _position + _index;

            // Check the next character is also a '>'
            if (((_index + 1) < _length) && (_line[_index + 1] == 62))
            {
                _index += 2;
                return TokenObject.DictionaryClose;
            }
            else
            {
                _index++;
                return new TokenError(position, $"Missing '>' after the initial '>'.");
            }
        }

        private TokenObject GetStringLiteral()
        {
            long position = _position + _index;

            // Move past the '(' start literal string marker
            _index++;

            int nesting = 0;
            int first = _index;
            int scanned = 0;
            bool continuation = false;
            bool checkedBOM = false;

            StringBuilder sb = new StringBuilder();

            // Keep scanning until we get to the end of literal string ')' marker
            while (true)
            {
                // Scan rest of the current line
                while (_index < _length)
                {
                    // Is this the start of an escape sequence
                    if (_line[_index] == 92)    // '\'
                    {
                        // If the last character, then '\' indicates that no newline should be appended into the literal string
                        if (_index >= (_length - 1))
                            continuation = true;
                        else
                        {
                            // Skip over the following escaped character for first digit of escaped number
                            _index++;
                        }
                    }
                    else if (_line[_index] == 41)   // ')'
                    {
                        // If the balancing end marker then we are finished
                        if (nesting == 0)
                        {
                            sb.Append(Encoding.ASCII.GetString(_line, first, _index - first));

                            // Move past the ')' marker
                            _index++;

                            return new TokenStringLiteral(sb.ToString());
                        }
                        else
                            nesting--;
                    }
                    else if (_line[_index] == 40)   // '('
                        nesting++;

                    _index++;
                    scanned++;

                    if (!checkedBOM && (scanned == 2))
                    {
                        // Check for the UTF16 Byte Order Mark (little endian or big endian versions)
                        if ((_line[_index - 2] == 0xFE) && (_line[_index - 1] == 0xFF))
                            return GetStringLiteralUTF16(position, true);
                        else if ((_line[_index - 2] == 0xFF) && (_line[_index - 1] == 0xFE))
                            return GetStringLiteralUTF16(position, false);
                    }
                }

                checkedBOM = true;

                if (continuation)
                {
                    // Append everything from the first character
                    sb.Append(Encoding.ASCII.GetString(_line, first, _index - first - 1));
                }
                else
                {
                    // Append everything from the first character but excluding the continuation marker
                    sb.Append(Encoding.ASCII.GetString(_line, first, _index - first));
                    sb.Append("\n");
                }

                _line = Reader.ReadLine();
                if (_line != null)
                {
                    _length = _line.Length;
                    _index = 0;
                    first = _index;
                    continuation = false;
                }
                else
                {
                    // End of content before end of string literal
                    return new TokenError(position, $"End of content before end of literal string character ')'.");
                }
            }
        }

        private TokenObject GetStringLiteralUTF16(long position, bool bigEndian)
        {
            int first = _index;
            byte temp;

            // Scan rest of the current line
            while (_index < _length)
            {
                // Is this the endof the literal
                if (_line[_index] == 41)    // ')'
                {
                    string literal = Encoding.Unicode.GetString(_line, first, _index - first);

                    // Move past the ')' marker
                    _index++;

                    return new TokenStringLiteral(literal);
                }

                _index += 2;

                if (bigEndian && (_index < _length))
                {
                    // Switch byte order of each character pair
                    temp = _line[_index - 1];
                    _line[_index - 1] = _line[_index - 2];
                    _line[_index - 2] = temp;
                }
            }

            // End of content before end of string literal
            return new TokenError(position, $"End of content before end of UTF16 literal string character.");
        }

        private string UniqueString(string str)
        {
            // Only keep a single instance of the same string value
            if (_uniqueStrings.TryGetValue(str, out string unique))
                return unique;
            else
            {
                _uniqueStrings.Add(str, str);
                return str;
            }
        }

        private Stream Stream { get; set; }
    }
}
