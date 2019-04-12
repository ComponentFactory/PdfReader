namespace PdfXenon.Standard
{
    public class ParseObjectReference : ParseObject
    {
        public ParseObjectReference(TokenInteger id, TokenInteger gen)
        {
            TokenId = id;
            TokenGen = gen;
        }

        public override string ToString()
        {
            return $"ParseObjectReference ({Position}): {Id},{Gen}";
        }

        public int Id { get => TokenId.Integer; }
        public int Gen { get => TokenGen.Integer; }
        public override long Position { get => TokenId.Position; }

        private TokenInteger TokenId { get; set; }
        private TokenInteger TokenGen { get; set; }
    }
}
