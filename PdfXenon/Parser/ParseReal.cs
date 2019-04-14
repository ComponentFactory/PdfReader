namespace PdfXenon.Standard
{
    public class ParseReal : ParseObject
    {
        public ParseReal(TokenReal token)
            : base(token.Position)
        {
            Value = token.Value;
        }

        public override string ToString()
        {
            return $"ParseReal ({Position}): {Value}";
        }

        public float Value { get; private set; }
    }
}
