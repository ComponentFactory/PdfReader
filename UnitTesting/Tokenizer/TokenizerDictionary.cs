using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerDictionary : HelperMethods
    {
        [Fact]
        public void DictionaryOpenFail()
        {
            Tokenizer t = new Tokenizer(StringToStream("<"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenFail2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenFail3()
        {
            Tokenizer t = new Tokenizer(StringToStream("< ["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenFail4()
        {
            Tokenizer t = new Tokenizer(StringToStream("< <"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenError);
        }

        [Fact]
        public void DictionaryCloseFail()
        {
            Tokenizer t = new Tokenizer(StringToStream(">"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryCloseFail2()
        {
            Tokenizer t = new Tokenizer(StringToStream(">["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryCloseFail3()
        {
            Tokenizer t = new Tokenizer(StringToStream("> ["));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryCloseFail4()
        {
            Tokenizer t = new Tokenizer(StringToStream("> >"));
            TokenError n = t.GetToken() as TokenError;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenError);
        }

        [Fact]
        public void DictionaryOpen()
        {
            Tokenizer t = new Tokenizer(StringToStream("<<"));
            TokenDictionaryOpen n = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n is TokenDictionaryOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryClose()
        {
            Tokenizer t = new Tokenizer(StringToStream(">>"));
            TokenDictionaryClose n = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n is TokenDictionaryClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryOpenClose()
        {
            Tokenizer t = new Tokenizer(StringToStream("<<>>"));

            TokenDictionaryOpen n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenDictionaryOpen);
            Assert.True(n1.Position == 0);

            TokenDictionaryClose n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2.Position == 2);
            Assert.True(n2 is TokenDictionaryClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void DictionaryNested()
        {
            Tokenizer t = new Tokenizer(StringToStream("<<<<>><<>>>>"));

            TokenDictionaryOpen n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenDictionaryOpen);
            Assert.True(n1.Position == 0);

            n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1.Position == 2);
            Assert.True(n1 is TokenDictionaryOpen);

            TokenDictionaryClose n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2.Position == 4);
            Assert.True(n2 is TokenDictionaryClose);

            n1 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(n1);
            Assert.True(n1.Position == 6);
            Assert.True(n1 is TokenDictionaryOpen);

            n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2.Position == 8);
            Assert.True(n2 is TokenDictionaryClose);

            n2 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(n2);
            Assert.True(n2.Position == 10);
            Assert.True(n2 is TokenDictionaryClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
