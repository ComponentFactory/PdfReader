namespace PdfXenon.Standard
{
    public class TokenDictionaryOpen : TokenObject
    {
        public TokenDictionaryOpen(long position)
            : base(position)
        {
        }
    }

    public class TokenDictionaryClose : TokenObject
    {
        public TokenDictionaryClose(long position)
            : base(position)
        {
        }
    }
}
