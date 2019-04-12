namespace PdfXenon.Standard
{
    public class TokenArrayOpen : TokenObject
    {
        public TokenArrayOpen(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"ArrayOpen ({Position})";
        }
    }

    public class TokenArrayClose : TokenObject
    {
        public TokenArrayClose(long position)
            : base(position)
        {
        }

        public override string ToString()
        {
            return $"ArrayClose ({Position})";
        }
    }
}
