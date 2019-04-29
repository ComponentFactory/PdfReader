namespace PdfXenon.Standard
{
    public class TokenReal : TokenObject
    {
        public TokenReal(float real)
        {
            Value = real;
        }

        public float Value { get; private set; }
    }
}
