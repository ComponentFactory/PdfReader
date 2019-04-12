namespace PdfXenon.Standard
{
    public class TokenEmpty : TokenObject
    {
        public TokenEmpty(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"Empty ({Position})";
        }
    }
}
