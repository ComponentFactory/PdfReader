namespace PdfXenon.Standard
{
    public abstract class TokenString : TokenObject
    {
        public TokenString(long position, string rawString)
            : base(position)
        {
            Raw = rawString;
        }

        public string Raw { get; private set; }

        public abstract string ResolvedAsString { get; }
        public abstract byte[] ResolvedAsBytes { get; }
    }
}
