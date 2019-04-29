using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
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
            Assert.True(c.Value == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentStart2()
        {
            Tokenizer t = new Tokenizer(StringToStream("  %comment"));
            t.IgnoreComments = false;

            TokenComment c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Value == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentMiddle()
        {
            Tokenizer t = new Tokenizer(StringToStream("\x00\x09\x0A\x0C\x0D%comment"));
            t.IgnoreComments = false;

            TokenComment c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Value == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void CommentThree()
        {
            Tokenizer t = new Tokenizer(StringToStream("%one\n%two\n%three"));
            t.IgnoreComments = false;

            TokenComment c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Value == "%one");

            c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Value == "%two");

            c = t.GetToken() as TokenComment;
            Assert.NotNull(c);
            Assert.True(c.Value == "%three");

            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
