namespace PdfXenon.Standard
{
    public class ParseBoolean : ParseObject
    {
        public ParseBoolean(TokenKeyword token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseBoolean ({Position}): {Value}";
        }

        public bool Value { get => Token.Keyword == ParseKeyword.True; }
        public override long Position { get => Token.Position; }

        private TokenKeyword Token { get; set; }
    }
}
