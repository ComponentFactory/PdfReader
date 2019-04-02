using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerComment : HelperMethods
    {
        [Fact]
        public void CommentIgnore()
        {
            Tokenizer t = new Tokenizer(StringToStream("%one\n%two\n%three"));
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentStart1()
        {
            Tokenizer t = new Tokenizer(StringToStream("%comment"));
            t.IgnoreComments = false;

            TokenComment c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%comment");
            Assert.True(c.Position == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentStart2()
        {
            Tokenizer t = new Tokenizer(StringToStream("  %comment"));
            t.IgnoreComments = false;

            TokenComment c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%comment");
            Assert.True(c.Position == 2);
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
            Assert.True(c.Position == 5);
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
            Assert.True(c.Position == 0);

            c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%two");
            Assert.True(c.Position == 5);

            c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Comment == "%three");
            Assert.True(c.Position == 10);

            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
