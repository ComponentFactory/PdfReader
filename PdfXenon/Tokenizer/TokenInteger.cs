namespace PdfXenon.Standard
{
    public class TokenInteger : TokenObject
    {
        public TokenInteger(long position, int integer)
            : base(position)
        {
            Value = integer;
        }

        public int Value { get; private set; }
    }
}
