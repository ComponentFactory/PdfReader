namespace PdfXenon.Standard
{
    public class ParseInteger : ParseObject
    {
        public ParseInteger(TokenInteger token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseInteger ({Position}): {Integer}";
        }

        public int Integer { get => Token.Integer; }
        public override long Position { get => Token.Position; }

        private TokenInteger Token { get; set; }
    }
}
