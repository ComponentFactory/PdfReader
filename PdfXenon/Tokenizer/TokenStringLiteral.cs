using System.Text;

namespace PdfXenon.Standard
{
    public class TokenStringLiteral : TokenString
    {
        private string _actual;

        public TokenStringLiteral(long position, string rawString)
            : base(position, rawString)
        {
        }

        public override string ResolvedAsString
        {
            get
            {
                if (_actual == null)
                {
                    StringBuilder sb = new StringBuilder();

                    string line = Raw;
                    int last = line.Length;
                    int first = 0;

                    for (int i = 0; i < last; i++)
                    {
                        // If we encounter an escape '\' that is not in the last character position
                        if ((line[i] == '\\') && (i < (last - 1)))
                        {
                            switch (line[i + 1])
                            {
                                case 'n':
                                case 'r':
                                case 't':
                                case 'b':
                                case 'f':
                                    // Convert from two characters to actual escaped character
                                    sb.Append(line.Substring(first, i - first));

                                    switch (line[i + 1])
                                    {
                                        case 'n':
                                            sb.Append("\n");
                                            break;
                                        case 'r':
                                            sb.Append("\r");
                                            break;
                                        case 't':
                                            sb.Append("\t");
                                            break;
                                        case 'b':
                                            sb.Append("\b");
                                            break;
                                        case 'f':
                                            sb.Append("\f");
                                            break;
                                    }

                                    i++;
                                    first = i + 1;
                                    break;
                                case '(':
                                case ')':
                                case '\\':
                                    // Ignore the escape '\' and then add the escaped character onwards
                                    sb.Append(line.Substring(first, i - first));

                                    i++;
                                    first = i;
                                    break;
                                case '0':
                                case '1':
                                case '2':
                                case '3':
                                case '4':
                                case '5':
                                case '6':
                                case '7':
                                    // Add preceding characters not already appended
                                    if (first < i)
                                    {
                                        sb.Append(line.Substring(first, i - first));
                                        first = i;
                                    }

                                    // Find all the octal digits
                                    byte octal = 0;
                                    for (int j = i + 1; j < last; j++)
                                    {
                                        char c = line[j];
                                        if ((c >= '0') && (c <= '7'))
                                        {
                                            octal *= 8;
                                            octal += (byte)(c - '0');

                                            i++;
                                            first = i + 1;
                                        }
                                        else
                                            break;
                                    }

                                    sb.Append((char)octal);
                                    break;
                            }
                        }
                    }

                    sb.Append(line.Substring(first, last - first));

                    _actual = sb.ToString();
                }
               
                return _actual;
            }
        }

        public override byte[] ResolvedAsBytes
        {
            get
            {
                return Encoding.ASCII.GetBytes(ResolvedAsString);
            }
        }
    }
}
