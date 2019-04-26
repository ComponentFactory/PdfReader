namespace PdfXenon.Standard
{
    public class TokenIdentifier : TokenObject
    {
        public TokenIdentifier(long position, string identifier)
            : base(position)
        {
            Value = identifier;
        }

        public string Value { get; private set; }
    }
}
