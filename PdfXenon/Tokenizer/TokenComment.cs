namespace PdfXenon.Standard
{
    public class TokenComment : TokenObject
    {
        public TokenComment(long position, string comment)
            : base(position)
        {
            Value = comment;
        }

        public override string ToString()
        {
            return $"Comment ({Position}): {Value} ";
        }

        public string Value { get; private set; }
    }
}
