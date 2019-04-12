namespace PdfXenon.Standard
{
    public class TokenInteger : TokenObject
    {
        public TokenInteger(long position, int integer)
            : base(position)
        {
            Integer = integer;
        }

        public override string ToString()
        {
            return $"Integer ({Position}): {Integer}";
        }

        public int Integer { get; set; }
    }
}
