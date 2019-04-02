using System;
using System.IO;
using System.Text;

namespace PdfXenon.Standard
{
    public class Tokenizer
    {
        private int _index;
        private int _length;
        private string _line;
        private long _position;
        private TokenReader _reader;

        public Tokenizer(Stream stream)
        {
            // Must have an actual stream reference
            Stream = stream ?? throw new ArgumentNullException("stream");

            // Stream is no use if we cannot read from it
            if (!stream.CanRead)
                throw new ApplicationException("cannot read from stream");
        }

        public bool IgnoreComments { get; set; } = true;

        public TokenBase GetToken()
        {
            TokenBase t = GetAnyToken();

            if (IgnoreComments)
                while (t is TokenComment)
                    t = GetAnyToken();

            return t;
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

        private bool HasLine()
        {
            return (_line != null) && (_index < _length);
        }

        private bool IsWhitespace(char c)
        {
            // TODO, convert to array bool lookup
            return (c == 0) ||      // Null
                   (c == 9) ||      // Tab
                   (c == 10) ||     // Line Feed
                   (c == 12) ||     // Form Feed
                   (c == 13) ||     // Carriage Return
                   (c == 32);       // Space
        }

        private bool IsDelimiter(char c)
        {
            // TODO, convert to array bool lookup
            return (c == '(') || (c == ')') ||
                   (c == '<') || (c == '>') ||
                   (c == '[') || (c == ']') ||
                   (c == '{') || (c == '}') ||
                   (c == '/') || (c == '%');
        }

        private bool IsHexadecimal(char c)
        {
            // TODO, convert to array bool lookup
            return ((c >= '0') && (c <= '9')) ||
                   ((c >= 'a') && (c <= 'f')) ||
                   ((c >= 'A') && (c <= 'F'));
        }

        private bool IsNumberStart(char c)
        {
            // TODO, convert to array bool lookup
            return ((c >= '0') && (c <= '9')) ||
                   (c == '+') ||
                   (c == '-') ||
                   (c == '.');
        }

        private bool IsHexadecimalOrWhitespace(char c)
        {
            return IsHexadecimal(c) || IsWhitespace(c);
        }

        private bool IsRegular(char c)
        {
            return !IsWhitespace(c) && !IsDelimiter(c);
        }

        private int HexToDigit(char c)
        {
            // TODO, convert to array integer lookup
            if ((c >= '0') && (c <= '9'))
                return c - '0';
            else if ((c >= 'a') && (c <= 'f'))
                return c - 'a' + 10;
            else
                return c - 'A' + 10;
        }

        private void SkipWhitespace()
        {
            while (true)
            {
                // Do we need to fetch the next line of characters?
                if (!HasLine())
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
                        // No more lines, we must be done
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
            if (!HasLine())
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
                    if (IsNumberStart(_line[_index]))
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
                            case '%':
                                return GetComment();
                            case '/':
                                return GetName();
                            case '<':
                                return GetDictionaryOpenOrHexString();
                            case '>':
                                return GetDictionaryClose();
                            case '(':
                                return GetStringLiteral();
                            case '[':
                                _index++;
                                return new TokenArrayOpen(position);
                            case ']':
                                _index++;
                                return new TokenArrayClose(position);
                        }
                    }

                    // Found invalid character for this position
                    return new TokenError(position, $"Cannot parse '{_line[_index]}' as a delimiter or whitespace.");
                }
            }
        }

        private TokenBase GetNumber(int end)
        {
            long position = _position + _index;
            string text = _line.Substring(_index, end - _index);
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
            string text = _line.Substring(_index, end - _index);
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
            string comment = _line.Substring(_index);

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

            string name = _line.Substring(_index + 1, end - _index - 1);
            _index = end;

            // Is there an escape sequence to process
            while (true)
            {
                int escape = name.IndexOf('#');
                if (escape >= 0)
                {
                    // Check there are two digits after it
                    if ((escape > (name.Length - 3)) || !IsHexadecimal(name[escape + 1]) || !IsHexadecimal(name[escape + 2]))
                        return new TokenError(position + escape, $"Escaped character in Name not followed by two hex digits.");

                    char val = (char)(HexToDigit(name[escape + 1]) * 16 + HexToDigit(name[escape + 2]));
                    name = name.Replace(name.Substring(escape, 3), $"{val}");
                }
                else
                    break;
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
                    return new TokenError(position, $"Missing closing '>' at end of hexadecimal string.");

                if (_line[end] != '>')
                    return new TokenError(position, $"Invalid character '{_line[end]}' found in hexadecimal string.");

                string str = _line.Substring(_index, end - _index);
                _index = end + 1;

                return new TokenHexString(position, str);
            }
        }

        private TokenBase GetDictionaryClose()
        {
            long position = _position + _index;

            // Check the next character is also a '>'
            if (((_index + 1) < _length) && (_line[_index + 1] == '>'))
            {
                _index += 2;
                return new TokenDictionaryClose(position);
            }
            else
            {
                _index++;
                return new TokenError(position, $"Expected another '>' after the initial '>'.");
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
                    if (_line[_index] == '\\')
                    {
                        // If the last character, then indicates no newline should be appended into the literal string
                        if (_index >= (_length - 1))
                            continuation = true;
                        else
                        {
                            // Skip over the following escaped character for first digit of escaped number
                            _index++;
                        }
                    }
                    else if (_line[_index] == ')')
                    {
                        // If the balancing end marker then we have finished
                        if (nesting == 0)
                        {
                            sb.Append(_line.Substring(first, _index - first));

                            // Move past the ')' marker
                            _index++;

                            return new TokenLiteralString(position, sb.ToString());
                        }
                        else
                            nesting--;
                    }
                    else if (_line[_index] == '(')
                        nesting++;

                    _index++;
                }

                if (continuation)
                {
                    // Append everything from the first character
                    sb.Append(_line.Substring(first, _index - first - 1));
                }
                else
                {
                    // Append everything from the first character but excluding the continuation marker
                    sb.Append(_line.Substring(first, _index - first));
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
