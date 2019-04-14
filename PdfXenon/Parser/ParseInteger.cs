namespace PdfXenon.Standard
{
    public class ParseInteger : ParseObject
    {
        public ParseInteger(TokenInteger token)
            : base(token.Position)
        {
            Value = token.Value;
        }

        public override string ToString()
        {
            return $"ParseInteger ({Position}): {Value}";
        }

        public int Value { get; private set; }
    }
}
