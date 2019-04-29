using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerArray : HelperMethods
    {
        [Fact]
        public void ArrayOpen1()
        {
            Tokenizer t = new Tokenizer(StringToStream("["));
            TokenArrayOpen n = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n);
            Assert.True(n is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpen2()
        {
            Tokenizer t = new Tokenizer(StringToStream("  ["));
            TokenArrayOpen n = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n);
            Assert.True(n is TokenArrayOpen);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayClose()
        {
            Tokenizer t = new Tokenizer(StringToStream("]"));
            TokenArrayClose n = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n);
            Assert.True(n is TokenArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpenClose1()
        {
            Tokenizer t = new Tokenizer(StringToStream("[]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpenClose2()
        {
            Tokenizer t = new Tokenizer(StringToStream(" [ ]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayNested()
        {
            Tokenizer t = new Tokenizer(StringToStream("[[][]]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);

            n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);

            n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);

            n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);

            n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
