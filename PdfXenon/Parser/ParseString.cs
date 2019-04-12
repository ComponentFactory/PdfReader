namespace PdfXenon.Standard
{
    public class ParseString : ParseObject
    {
        public ParseString(TokenString token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseString ({Position}): {String}";
        }

        public string String
        {
            get { return Token.ResolvedString; }
        }

        public override long Position { get => Token.Position; }

        private TokenString Token { get; set; }
    }
}
