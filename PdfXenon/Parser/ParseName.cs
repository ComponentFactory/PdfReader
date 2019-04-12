namespace PdfXenon.Standard
{
    public class ParseName : ParseObject
    {
        public ParseName(TokenName token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"ParseName ({Position}): {Name}";
        }

        public string Name { get => Token.Name; }
        public override long Position { get => Token.Position; }

        private TokenName Token { get; set; }
    }
}
