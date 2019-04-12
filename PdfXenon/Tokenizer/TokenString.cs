namespace PdfXenon.Standard
{
    public abstract class TokenString : TokenObject
    {
        public TokenString(long position, string rawString)
            : base(position)
        {
            RawString = rawString;
        }

        public string RawString { get; private set; }
        public abstract string ResolvedString { get; }
    }
}
