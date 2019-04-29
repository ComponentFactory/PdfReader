namespace PdfXenon.Standard
{
    public class TokenXRefEntry : TokenObject
    {
        public TokenXRefEntry(int id, int gen, long offset, bool used)
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
