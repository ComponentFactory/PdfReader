using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class TokenBase
    {
        public static TokenEmpty Empty = new TokenEmpty();
        public static TokenArrayOpen ArrayOpen = new TokenArrayOpen();
        public static TokenArrayClose ArrayClose = new TokenArrayClose();
        public static TokenDictionaryOpen DictionaryOpen = new TokenDictionaryOpen();
        public static TokenDictionaryClose DictionaryClose = new TokenDictionaryClose();
    }
}
