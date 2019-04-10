﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Tokenizer
    {
        // Lookup arrays are fast and simple when the source is ASCII characters and so limimted to 256 values
        private static bool[] _whitespaceLookup;
        private static byte[] _whitespace = new byte[] { 0, 9, 10, 12,13, 32 };
        private static bool[] _delimiterLookup;
        private static byte[] _delimiter = new byte[] { 40, 41, 60, 62, 91, 93, 123, 125, 47, 37 };
        private static bool[] _delimiterOrWhitespaceLookup;
        private static int[] _hexToDecimalLookup;
        private static bool[] _hexadecimalLookup;
        private static bool[] _hexadecimalOrWhitespaceLookup;
        private static byte[] _hexadecimal = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 65, 66, 67, 68, 69, 70, 97, 98, 99, 100,101, 102 };
        private static bool[] _isStartOfNumberLookup;
        private static byte[] _isNumberStart = new byte[] { 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 43, 45, 46 };

        private int _index;
        private int _length;
        private long _position;
        private byte[] _line;
        private TokenReader _reader;
        private Stack<TokenBase> _stack = new Stack<TokenBase>();

        static Tokenizer()
        {
            _whitespaceLookup = new bool[256];
            foreach (byte code in _whitespace)
                _whitespaceLookup[code] = true;

            _delimiterLookup = new bool[256];
            foreach (byte code in _delimiter)
                _delimiterLookup[code] = true;

            _hexadecimalLookup = new bool[256];
            foreach (byte code in _hexadecimal)
                _hexadecimalLookup[code] = true;

            _isStartOfNumberLookup = new bool[256];
            foreach (byte code in _isNumberStart)
                _isStartOfNumberLookup[code] = true;

            _hexadecimalOrWhitespaceLookup = new bool[256];
            foreach (byte code in _whitespace)
                _hexadecimalOrWhitespaceLookup[code] = true;
            foreach (byte code in _hexadecimal)
                _hexadecimalOrWhitespaceLookup[code] = true;

            _delimiterOrWhitespaceLookup = new bool[256];
            foreach (byte code in _whitespace)
                _delimiterOrWhitespaceLookup[code] = true;
            foreach (byte code in _delimiter)
                _delimiterOrWhitespaceLookup[code] = true;

            _hexToDecimalLookup = new int[256];
            for (int i = 0; i < 10; i++)
                _hexToDecimalLookup[48 + i] = i;
            for (int i=0; i<6; i++)
            {
                _hexToDecimalLookup[97 + i] = i;
                _hexToDecimalLookup[65 + i] = i;
            }
        }

        public Tokenizer(Stream stream)
        {
            // Must have an actual stream reference
            Stream = stream ?? throw new ArgumentNullException("stream");

            // Stream is no use if we cannot read from it
            if (!stream.CanRead)
                throw new ApplicationException("cannot read from stream");
        }

        public bool IgnoreComments { get; set; } = true;

        public void PushToken(TokenBase token)
        {
            _stack.Push(token);
        }

        public TokenBase GetToken()
        {
            TokenBase t = _stack.Count > 0 ? _stack.Pop() : GetAnyToken();

            if (IgnoreComments)
                while (t is TokenComment)
                    t = _stack.Count > 0 ? _stack.Pop() : GetAnyToken();

            return t;
        }

        public byte[] GetBytes(int length)
        {
            return Reader.GetBytes(length);
        }

        private Stream Stream { get; set; }

        private TokenReader Reader
        {
            get
            {
                if (_reader == null)
                    _reader = new TokenReader(Stream);

                return _reader;
            }
        }

        private bool IsWhitespace(byte c)
        {
            return _whitespaceLookup[c];
        }

        private bool IsDelimiter(byte c)
        {
            return _delimiterLookup[c];
        }

        private bool IsRegular(byte c)
        {
            return !_delimiterOrWhitespaceLookup[c];
        }

        private bool IsHexadecimal(byte c)
        {
            return _hexadecimalLookup[c];
        }

        private bool IsHexadecimalOrWhitespace(byte c)
        {
            return _hexadecimalOrWhitespaceLookup[c];
        }

        private bool IsStartOfNumber(byte c)
        {
            return _isStartOfNumberLookup[c];
        }

        private int ConvertHexToDecimal(byte c)
        {
            return _hexToDecimalLookup[c];
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
                    if (IsWhitespace(_line[_index]))
                        _index++;
                    else
                        return;
                }
            }
        }

        private TokenBase GetAnyToken()
        {
            // Ignore zero or more whitespace characters
            SkipWhitespace();

            // Have we run out of content?
            if ((_line == null) || (_index == _length))
                return new TokenEmpty(_position);
            else
            {
                long position = _position + _index;

                // Find the run of regular characters
                int end = _index;
                while ((end < _length) && IsRegular(_line[end]))
                    end++;

                // If at least one regular character
                if (end > _index)
                {
                    if (IsStartOfNumber(_line[_index]))
                        return GetNumber(end);
                    else
                        return GetKeyword(end);
                }
                else
                {
                    // Must have found a delimiter instead
                    if (IsDelimiter(_line[_index]))
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
                                return new TokenArrayOpen(position);
                            case 93: // ']'
                                _index++;
                                return new TokenArrayClose(position);
                        }
                    }

                    // Found invalid character for this position
                    return new TokenError(position, $"Cannot parse '{_line[_index]}' as a delimiter or regular character.");
                }
            }
        }

        private TokenBase GetNumber(int end)
        {
            long position = _position + _index;
            string text = new string(Encoding.ASCII.GetChars(_line, _index, end - _index));
            _index = end;

            if (int.TryParse(text, out int integer))
                return new TokenNumeric(position, integer);
            else
            {
                if (double.TryParse(text, out double real))
                    return new TokenNumeric(position, real);
                else
                {
                    // String is not a recognized number format
                    return new TokenError(position, $"Cannot parse '{text}' as a number.");
                }
            }
        }

        private TokenBase GetKeyword(int end)
        {
            long position = _position + _index;
            string text = new string(Encoding.ASCII.GetChars(_line, _index, end - _index));
            _index = end;

            TokenKeyword token = TokenKeyword.CheckKeywords(position, text);
            if (token != null)
                return token;

            // String is not a recognized keyword
            return new TokenError(position, $"Cannot parse '{text}' as a keyword.");
        }

        private TokenBase GetComment()
        {
            long position = _position + _index;

            // Everything till end of the line is the comment
            string comment = new string(Encoding.ASCII.GetChars(_line, _index, _length - _index));

            // Continue processing at start of the next line
            _line = null;

            return new TokenComment(position, comment);
        }

        private TokenBase GetName()
        {
            long position = _position + _index;

            // Find the run of regular characters
            int end = _index + 1;
            while ((end < _length) && IsRegular(_line[end]))
                end++;

            string name = new string(Encoding.ASCII.GetChars(_line, _index + 1, end - _index - 1));
            _index = end;

            // Convert any escape sequences
            while (true)
            {
                int escape = name.IndexOf('#');
                if (escape < 0)
                    break;

                // Check there are two digits after it
                if ((escape > (name.Length - 3)) || !IsHexadecimal((byte)name[escape + 1]) || !IsHexadecimal((byte)name[escape + 2]))
                    return new TokenError(position + escape, $"Escaped character inside name is not followed by two hex digits.");

                char val = (char)(ConvertHexToDecimal((byte)name[escape + 1]) * 16 + ConvertHexToDecimal((byte)name[escape + 2]));
                name = name.Replace(name.Substring(escape, 3), $"{val}");
            }

            return new TokenName(position, name);
        }

        private TokenBase GetDictionaryOpenOrHexString()
        {
            long position = _position + _index;

            _index++;
            if (_index >= _length)
                return new TokenError(position, $"Unexpected end of line after '<'.");

            // Is the next character another '<'
            if (_line[_index] == '<')
            {
                _index++;
                return new TokenDictionaryOpen(position);
            }
            else
            {
                // Find the run of hexadecimal characters and whitespace
                int end = _index;
                while ((end < _length) && IsHexadecimalOrWhitespace(_line[end]))
                    end++;

                if (end == _length)
                    return new TokenError(position, $"Missing '>' at end of hexadecimal string.");

                if (_line[end] != '>')
                    return new TokenError(position, $"Invalid character '{_line[end]}' found in hexadecimal string.");

                string str = new string(Encoding.ASCII.GetChars(_line, _index, end - _index));
                _index = end + 1;

                return new TokenHexString(position, str);
            }
        }

        private TokenBase GetDictionaryClose()
        {
            long position = _position + _index;

            // Check the next character is also a '>'
            if (((_index + 1) < _length) && (_line[_index + 1] == 62))
            {
                _index += 2;
                return new TokenDictionaryClose(position);
            }
            else
            {
                _index++;
                return new TokenError(position, $"Missing '>' after the initial '>'.");
            }
        }

        private TokenBase GetStringLiteral()
        {
            long position = _position + _index;

            // Move past the '(' start literal string marker
            _index++;

            int nesting = 0;
            int first = _index;
            bool continuation = false;

            StringBuilder sb = new StringBuilder();

            // Keep scanning until we get to the end of literal string ')' marker
            while (true)
            {
                // Scan rest of the current line
                while (_index < _length)
                {
                    // Is this the start of an escape sequence
                    if (_line[_index] == 92)
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
                    else if (_line[_index] == 41)
                    {
                        // If the balancing end marker then we are finished
                        if (nesting == 0)
                        {
                            sb.Append(new string(Encoding.ASCII.GetChars(_line, first, _index - first)));

                            // Move past the ')' marker
                            _index++;

                            return new TokenLiteralString(position, sb.ToString());
                        }
                        else
                            nesting--;
                    }
                    else if (_line[_index] == 40)
                        nesting++;

                    _index++;
                }

                if (continuation)
                {
                    // Append everything from the first character
                    sb.Append(new string(Encoding.ASCII.GetChars(_line, first, _index - first - 1)));
                }
                else
                {
                    // Append everything from the first character but excluding the continuation marker
                    sb.Append(new string(Encoding.ASCII.GetChars(_line, first, _index - first)));
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
    }
}
