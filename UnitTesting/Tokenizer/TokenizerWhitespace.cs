using PdfReader;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerWhitespace : HelperMethods
    {
        [Fact]
        public void WhitespaceNull()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x00"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceTab()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x09"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceLineFeed()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x0A"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceFormFeed()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x0C"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceCarriageReturn()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x0D"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceSpace()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x20"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceCharacters()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x00\x09\x0A\x0C\x0D\x20\x0D\x0C\x0A\x09\x00"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void WhitespaceString()
        {
            Tokenizer t = new Tokenizer(StringToStream("   \r\n\x09\x09\x09\r\n\x09   \x09"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
