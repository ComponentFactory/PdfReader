namespace PdfXenon.Standard
{
    public class ParseNull : ParseObject
    {
        public ParseNull(TokenKeyword token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseNull ({Position})";
        }

        public override long Position { get => Token.Position; }

        private TokenKeyword Token { get; set; }
    }
}
