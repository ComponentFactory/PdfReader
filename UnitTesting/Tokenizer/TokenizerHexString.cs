using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerHexString
    {
        [Fact]
        public void HexString1()
        {
            Tokenizer t = new Tokenizer(StringToStream("<00>"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.HexString == "00");
            Assert.True(s.IsHex);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<6465>"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.HexString == "6465");
            Assert.True(s.IsHex);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1Whitespace()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 0 0 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.HexString == " 0 0 ");
            Assert.True(s.IsHex);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString2Whitespace()
        {
            Tokenizer t = new Tokenizer(StringToStream("<64 65 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.HexString == "64 65 ");
            Assert.True(s.IsHex);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexStringTruncated1()
        {
            Tokenizer t = new Tokenizer(StringToStream("<"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
        }

        [Fact]
        public void HexStringTruncated2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<00"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
        }

        [Fact]
        public void HexStringTruncated3()
        {
            Tokenizer t = new Tokenizer(StringToStream("<00["));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
        }

        private MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
