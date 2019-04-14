namespace PdfXenon.Standard
{
    public class ParseIndirectObject : ParseObject
    {
        public ParseIndirectObject(TokenInteger id, TokenInteger gen, ParseObject obj)
            : base(id.Position)
        {
            Id = id.Value;
            Gen = gen.Value;
            Object = obj;
        }

        public override string ToString()
        {
            return $"ParseIndirectObject ({Position}): {Id},{Gen} Child:{Object.ToString()}";
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public ParseObject Object { get; private set; }
    }
}
