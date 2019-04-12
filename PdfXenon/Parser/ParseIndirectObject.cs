namespace PdfXenon.Standard
{
    public class ParseIndirectObject : ParseObject
    {
        public ParseIndirectObject(TokenInteger id, TokenInteger gen, ParseObject obj)
        {
            TokenId = id;
            TokenGen = gen;
            Child = obj;
        }

        public override string ToString()
        {
            return $"ParseIndirectObject ({Position}): {Id},{Gen} Child:{Child.ToString()}";
        }

        public int Id { get => TokenId.Integer; }
        public int Gen { get => TokenGen.Integer; }
        public ParseObject Child { get; private set; }
        public override long Position { get => TokenId.Position; }

        private TokenInteger TokenId { get; set; }
        private TokenInteger TokenGen { get; set; }
    }
}
