namespace PdfXenon.Standard
{
    public class TokenComment : TokenObject
    {
        public TokenComment(long position, string comment)
            : base(position)
        {
            Comment = comment;
        }

        public override string ToString()
        {
            return $"Comment ({Position}): {Comment} ";
        }

        public string Comment { get; private set; }
    }
}
