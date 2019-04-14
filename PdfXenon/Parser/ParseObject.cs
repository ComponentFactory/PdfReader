namespace PdfXenon.Standard
{
    public abstract class ParseObject
    {
        public ParseObject(long position)
        {
            Position = position;
        }

        public long Position { get; protected set; }
    }
}
