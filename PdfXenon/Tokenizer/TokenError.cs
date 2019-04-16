namespace PdfXenon.Standard
{
    public class TokenError : TokenObject
    {
        public TokenError(long position, string message)
            : base(position)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
