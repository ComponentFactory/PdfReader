using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerName : HelperMethods
    {
        [Fact]
        public void NameZeroLength1()
        {
            Tokenizer t = new Tokenizer(StringToStream("/"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameZeroLength2()
        {
            Tokenizer t = new Tokenizer(StringToStream("/ "));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameSimple()
        {
            Tokenizer t = new Tokenizer(StringToStream("/Name"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "Name");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NamePosition()
        {
            Tokenizer t = new Tokenizer(StringToStream("   /Name   "));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 3);
            Assert.True(n.Name == "Name");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameSpecialChars()
        {
            Tokenizer t = new Tokenizer(StringToStream("/A;_-*B?"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "A;_-*B?");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameLikeNumber()
        {
            Tokenizer t = new Tokenizer(StringToStream("/1.2"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "1.2");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameEscaped1()
        {
            Tokenizer t = new Tokenizer(StringToStream("/A#20B"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "A B");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameEscaped2()
        {
            Tokenizer t = new Tokenizer(StringToStream("/A#20#20#20B"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "A   B");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NameEscaped3()
        {
            Tokenizer t = new Tokenizer(StringToStream("/A#28B#29"));
            TokenName n = t.GetToken() as TokenName;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Name == "A(B)");
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
