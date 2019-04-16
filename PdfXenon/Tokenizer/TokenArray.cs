namespace PdfXenon.Standard
{
    public class TokenArrayOpen : TokenObject
    {
        public TokenArrayOpen(long position)
            : base(position)
        {
        }
    }

    public class TokenArrayClose : TokenObject
    {
        public TokenArrayClose(long position)
            : base(position)
        {
        }
    }
}
