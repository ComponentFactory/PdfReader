using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerKeyword : HelperMethods
    {
        [Fact]
        public void KeywordFail()
        {
            Tokenizer t = new Tokenizer(StringToStream("random"));
            TokenBase k = t.GetToken();
            Assert.NotNull(k);
            Assert.True(k is TokenError);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordTrue()
        {
            Tokenizer t = new Tokenizer(StringToStream("true"));
            TokenKeyword k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.True);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordTrueIncorrect()
        {
            Tokenizer t = new Tokenizer(StringToStream("True"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordFalse()
        {
            Tokenizer t = new Tokenizer(StringToStream("false"));
            TokenKeyword k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.False);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordFalseIncorrect()
        {
            Tokenizer t = new Tokenizer(StringToStream("False"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordNull()
        {
            Tokenizer t = new Tokenizer(StringToStream("null"));
            TokenKeyword k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.Null);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordNullIncorrect()
        {
            Tokenizer t = new Tokenizer(StringToStream("Null"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void KeywordMultiple()
        {
            Tokenizer t = new Tokenizer(StringToStream("true false true false null"));

            TokenKeyword k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.True);

            k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.False);

            k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.True);

            k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.False);

            k = t.GetToken() as TokenKeyword;
            Assert.NotNull(k);
            Assert.True(k == TokenKeyword.Null);

            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
