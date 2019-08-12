namespace PdfReader
{
    public class TokenComment : TokenObject
    {
        public TokenComment(string comment)
        {
            Value = comment;
        }

        public string Value { get; private set; }
    }
}
