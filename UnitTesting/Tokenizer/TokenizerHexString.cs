using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerHexString : HelperMethods
    {
        [Fact]
        public void HexString1()
        {
            Tokenizer t = new Tokenizer(StringToStream("<20>"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == "20");
            Assert.True(s.ActualString == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceA()
        {
            Tokenizer t = new Tokenizer(StringToStream("<20 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == "20 ");
            Assert.True(s.ActualString == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceB()
        {
            Tokenizer t = new Tokenizer(StringToStream("<2 0 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == "2 0 ");
            Assert.True(s.ActualString == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceC()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 2 0 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == " 2 0 ");
            Assert.True(s.ActualString == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceD()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 20 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == " 20 ");
            Assert.True(s.ActualString == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceE()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 20>"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == " 20");
            Assert.True(s.ActualString == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<6465>"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == "6465");
            Assert.True(s.ActualString == "de");
            Assert.True(t.GetToken() is TokenEmpty);
        }


        [Fact]
        public void HexString2Whitespace()
        {
            Tokenizer t = new Tokenizer(StringToStream("<64 65 >"));
            TokenString s = t.GetToken() as TokenString;
            Assert.NotNull(s);
            Assert.True(s.IsHex);
            Assert.True(s.HexString == "64 65 ");
            Assert.True(s.ActualString == "de");
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
    }
}
