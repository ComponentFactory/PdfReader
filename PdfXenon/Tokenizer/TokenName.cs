namespace PdfXenon.Standard
{
    public class TokenName : TokenObject
    {
        public TokenName(long position, string name)
            : base(position)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"Name ({Position}): {Name}";
        }

        public string Name { get; private set; }
    }
}
