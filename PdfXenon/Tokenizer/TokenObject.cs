namespace PdfXenon.Standard
{
    public abstract class TokenObject
    {
        public TokenObject(long position)
        {
            Position = position;
        }

        public long Position { get; protected set; }
    }
}
