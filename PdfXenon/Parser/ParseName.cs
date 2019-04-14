namespace PdfXenon.Standard
{
    public class ParseName : ParseObject
    {
        public ParseName(TokenName token)
            : base(token.Position)
        {
            Value = token.Value;
        }

        public override string ToString()
        {
            return $"ParseName ({Position}): {Value}";
        }

        public string Value { get; private set; }
    }
}
