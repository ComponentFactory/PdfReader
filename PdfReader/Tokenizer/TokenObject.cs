namespace PdfReader
{
    public abstract class TokenObject
    {
        public static readonly TokenArrayOpen ArrayOpen = new TokenArrayOpen();
        public static readonly TokenArrayClose ArrayClose = new TokenArrayClose();
        public static readonly TokenDictionaryOpen DictionaryOpen = new TokenDictionaryOpen();
        public static readonly TokenDictionaryClose DictionaryClose = new TokenDictionaryClose();
        public static readonly TokenEmpty Empty = new TokenEmpty();

        public TokenObject()
        {
        }
    }

    public class TokenArrayOpen : TokenObject
    {
        public TokenArrayOpen()
        {
        }
    }

    public class TokenArrayClose : TokenObject
    {
        public TokenArrayClose()
        {
        }
    }

    public class TokenDictionaryOpen : TokenObject
    {
        public TokenDictionaryOpen()
        {
        }
    }

    public class TokenDictionaryClose : TokenObject
    {
        public TokenDictionaryClose()
        {
        }
    }

    public class TokenEmpty : TokenObject
    {
        public TokenEmpty()
        {
        }
    }
}
