namespace PdfReader
{
    public class TokenError : TokenObject
    {
        public TokenError(long position, string message)
        {
            Position = position;
            Message = message;
        }

        public long Position { get; private set; }
        public string Message { get; private set; }
    }
}
