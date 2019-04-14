namespace PdfXenon.Standard
{
    public class ParseBoolean : ParseObject
    {
        public ParseBoolean(TokenKeyword token)
            : base(token.Position)
        {
            Value = (token.Value == ParseKeyword.True);
        }

        public override string ToString()
        {
            return $"ParseBoolean ({Position}): {Value}";
        }

        public bool Value { get; private set; }
    }
}
