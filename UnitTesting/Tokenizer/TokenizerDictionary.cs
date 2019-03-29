using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerDictionary
    {
        [Fact]
        public void DictionaryOpenFail()
        {
            Tokenizer t = new Tokenizer(StringToStream("<"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenFail2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenFail3()
        {
            Tokenizer t = new Tokenizer(StringToStream("< ["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenFail4()
        {
            Tokenizer t = new Tokenizer(StringToStream("< <"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenError);
        }

        [Fact]
        public void DictionaryCloseFail()
        {
            Tokenizer t = new Tokenizer(StringToStream(">"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryCloseFail2()
        {
            Tokenizer t = new Tokenizer(StringToStream(">["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryCloseFail3()
        {
            Tokenizer t = new Tokenizer(StringToStream("> ["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryCloseFail4()
        {
            Tokenizer t = new Tokenizer(StringToStream("> >"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(t.GetToken() is TokenError);
        }

        [Fact]
        public void DictionaryOpen()
        {
            Tokenizer t = new Tokenizer(StringToStream("<<"));
            TokenDictionaryOpen n = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n);
            Assert.True(n == TokenBase.DictionaryOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryClose()
        {
            Tokenizer t = new Tokenizer(StringToStream(">>"));
            TokenDictionaryClose n = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n);
            Assert.True(n == TokenBase.DictionaryClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenClose()
        {
            Tokenizer t = new Tokenizer(StringToStream("<<>>"));

            TokenDictionaryOpen n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.DictionaryOpen);

            TokenDictionaryClose n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.DictionaryClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryNested()
        {
            Tokenizer t = new Tokenizer(StringToStream("<<<<>><<>>>>"));

            TokenDictionaryOpen n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.DictionaryOpen);

            n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.DictionaryOpen);

            TokenDictionaryClose n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.DictionaryClose);

            n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.DictionaryOpen);

            n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.DictionaryClose);

            n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.DictionaryClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        private MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
