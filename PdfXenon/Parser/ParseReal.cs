namespace PdfXenon.Standard
{
    public class ParseReal : ParseObject
    {
        public ParseReal(TokenReal token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseReal ({Position}): {Real}";
        }

        public float Real { get => Token.Real; }
        public override long Position { get => Token.Position; }

        private TokenReal Token { get; set; }
    }
}
