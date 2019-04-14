namespace PdfXenon.Standard
{
    public class TokenInteger : TokenObject
    {
        public TokenInteger(long position, int integer)
            : base(position)
        {
            Value = integer;
        }

        public override string ToString()
        {
            return $"Integer ({Position}): {Value}";
        }

        public int Value { get; set; }
    }
}
