using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerArray
    {
        [Fact]
        public void ArrayOpen()
        {
            Tokenizer t = new Tokenizer(StringToStream("["));
            TokenArrayOpen n = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n);
            Assert.True(n == TokenBase.ArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayClose()
        {
            Tokenizer t = new Tokenizer(StringToStream("]"));
            TokenArrayClose n = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n);
            Assert.True(n == TokenBase.ArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpenClose()
        {
            Tokenizer t = new Tokenizer(StringToStream("[]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.ArrayOpen);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.ArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayNested()
        {
            Tokenizer t = new Tokenizer(StringToStream("[[][]]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.ArrayOpen);

            n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.ArrayOpen);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.ArrayClose);

            n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 == TokenBase.ArrayOpen);

            n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.ArrayClose);

            n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 == TokenBase.ArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        private MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
