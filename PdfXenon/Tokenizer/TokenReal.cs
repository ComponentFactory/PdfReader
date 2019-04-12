namespace PdfXenon.Standard
{
    public class TokenReal : TokenObject
    {
        public TokenReal(long position, float real)
            : base(position)
        {
            Real = real;
        }

        public override string ToString()
        {
            return $"Real ({Position}): {Real}";
        }

        public float Real { get; set; }
    }
}
