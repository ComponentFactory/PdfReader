namespace PdfXenon.Standard
{
    public class ParseString : ParseObject
    {
        public ParseString(TokenString token)
            : base(token.Position)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseString ({Position}): {Value}";
        }

        public string Value
        {
            get { return Token.Resolved; }
        }

        private TokenString Token { get; set; }
    }
}
