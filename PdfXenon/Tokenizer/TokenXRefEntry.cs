namespace PdfXenon.Standard
{
    public class TokenXRefEntry : TokenObject
    {
        public TokenXRefEntry(long position, int id, int gen, long offset, bool used)
            : base(position)
        {
            Id = id;
            Gen = gen;
            Offset = offset;
            Used = used;
        }

        public int Id { get; private set; }
        public int Gen { get; private set; }
        public long Offset { get; private set; }
        public bool Used { get; private set; }
    }
}
