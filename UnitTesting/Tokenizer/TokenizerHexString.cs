using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerHexString : HelperMethods
    {
        [Fact]
        public void HexString1()
        {
            Tokenizer t = new Tokenizer(StringToStream("<20>"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == "20");
            Assert.True(s.Resolved == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexStringDouble()
        {
            Tokenizer t = new Tokenizer(StringToStream("<20><64>"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == "20");
            Assert.True(s.Resolved == " ");

            s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 4);
            Assert.True(s.Raw == "64");
            Assert.True(s.Resolved == "d");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceA()
        {
            Tokenizer t = new Tokenizer(StringToStream("<20 >"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == "20 ");
            Assert.True(s.Resolved == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceB()
        {
            Tokenizer t = new Tokenizer(StringToStream("<2 0 >"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == "2 0 ");
            Assert.True(s.Resolved == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceC()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 2 0 >"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == " 2 0 ");
            Assert.True(s.Resolved == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceD()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 20 >"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == " 20 ");
            Assert.True(s.Resolved == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString1WhitespaceE()
        {
            Tokenizer t = new Tokenizer(StringToStream("< 20>"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == " 20");
            Assert.True(s.Resolved == " ");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexString2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<6465>"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == "6465");
            Assert.True(s.Resolved == "de");
            Assert.True(t.GetToken() is TokenEmpty);
        }


        [Fact]
        public void HexString2Whitespace()
        {
            Tokenizer t = new Tokenizer(StringToStream("<64 65 >"));
            TokenStringHex s = t.GetToken() as TokenStringHex;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.Raw == "64 65 ");
            Assert.True(s.Resolved == "de");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void HexStringTruncated1()
        {
            Tokenizer t = new Tokenizer(StringToStream("<"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
            Assert.True(e.Position == 0);
        }

        [Fact]
        public void HexStringTruncated2()
        {
            Tokenizer t = new Tokenizer(StringToStream("<00"));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
            Assert.True(e.Position == 0);
        }

        [Fact]
        public void HexStringTruncated3()
        {
            Tokenizer t = new Tokenizer(StringToStream("<00["));
            TokenError e = t.GetToken() as TokenError;
            Assert.NotNull(e);
            Assert.True(e.Position == 0);
        }
    }
}
