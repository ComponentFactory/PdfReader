namespace PdfXenon.Standard
{
    public class ParseNull : ParseObject
    {
        public ParseNull(TokenKeyword token)
            : base(token.Position)
        {
        }

        public override string ToString()
        {
            return $"ParseNull ({Position})";
        }
    }
}
