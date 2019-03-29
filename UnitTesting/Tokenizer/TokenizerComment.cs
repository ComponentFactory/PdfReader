using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerComment
    {
        [Fact]
        public void CommentIgnore()
        {
            Tokenizer t = new Tokenizer(StringToStream("%one\n%two\n%three"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentStart()
        {
            Tokenizer t = new Tokenizer(StringToStream("%comment"));
            t.IgnoreComments = false;
            TokenComment c = t.GetToken() as TokenComment;

            Assert.NotNull(c);
            Assert.True(c.Comment == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentMiddle()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x00\x09\x0A\x0C\x0D%comment"));
            t.IgnoreComments = false;
            TokenComment c = t.GetToken() as TokenComment;

            Assert.NotNull(c);
            Assert.True(c.Comment == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentThree()
        {
            Tokenizer t = new Tokenizer(StringToStream("%one\n%two\n%three"));
            t.IgnoreComments = false;

            TokenComment c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%one");

            c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%two");

            c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%three");

            Assert.True(t.GetToken() is TokenEmpty);
        }

        private MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
