namespace PdfReader
{
    public class TokenInteger : TokenObject
    {
        public TokenInteger(int integer)
        {
            Value = integer;
        }

        public int Value { get; private set; }
    }
}
