namespace PdfXenon.Standard
{
    public class TokenName : TokenObject
    {
        public TokenName(long position, string name)
            : base(position)
        {
            Value = name;
        }

        public override string ToString()
        {
            return $"Name ({Position}): {Value}";
        }

        public string Value { get; private set; }
    }
}
