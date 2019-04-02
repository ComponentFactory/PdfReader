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
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpen2()
        {
            Tokenizer t = new Tokenizer(StringToStream("  ["));
            TokenArrayOpen n = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n);
            Assert.True(n is TokenArrayOpen);
            Assert.True(n.Position == 2);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayClose()
        {
            Tokenizer t = new Tokenizer(StringToStream("]"));
            TokenArrayClose n = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n);
            Assert.True(n is TokenArrayClose);
            Assert.True(n.Position == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpenClose1()
        {
            Tokenizer t = new Tokenizer(StringToStream("[]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);
            Assert.True(n1.Position == 0);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(n2.Position == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayOpenClose2()
        {
            Tokenizer t = new Tokenizer(StringToStream(" [ ]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);
            Assert.True(n1.Position == 1);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(n2.Position == 3);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void ArrayNested()
        {
            Tokenizer t = new Tokenizer(StringToStream("[[][]]"));

            TokenArrayOpen n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);
            Assert.True(n1.Position == 0);

            n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);
            Assert.True(n1.Position == 1);

            TokenArrayClose n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(n2.Position == 2);

            n1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(n1);
            Assert.True(n1 is TokenArrayOpen);
            Assert.True(n1.Position == 3);

            n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(n2.Position == 4);

            n2 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(n2);
            Assert.True(n2 is TokenArrayClose);
            Assert.True(n2.Position == 5);
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
