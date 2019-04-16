namespace PdfXenon.Standard
{
    public class TokenReal : TokenObject
    {
        public TokenReal(long position, float real)
            : base(position)
        {
            Value = real;
        }

        public float Value { get; set; }
    }
}
