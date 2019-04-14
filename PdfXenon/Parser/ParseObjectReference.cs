namespace PdfXenon.Standard
{
    public class ParseObjectReference : ParseObject
    {
        public ParseObjectReference(TokenInteger id, TokenInteger gen)
            : base(id.Position)
        {
            Id = id.Value;
            Gen = gen.Value;
        }

        public override string ToString()
        {
            return $"ParseObjectReference ({Position}): {Id},{Gen}";
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
    }
}
