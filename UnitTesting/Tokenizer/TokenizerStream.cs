using PdfReader;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerStream : HelperMethods
    {
        [Fact]
        public void EmptyStream()
        {
            MemoryStream ms = new MemoryStream(new byte[] { });
            Tokenizer t = new Tokenizer(ms);
            Assert.True(t.GetToken() is TokenEmpty);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void OneOfEach()
        {
            Tokenizer t = new Tokenizer(StringToStream("[<6465><<true/Name 1 3.14 >>]%comment"));
            t.IgnoreComments = false;

            TokenArrayOpen a1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(a1);

            TokenStringHex a2 = t.GetToken() as TokenStringHex;
            Assert.NotNull(a2);
            Assert.True(a2.Raw == "6465");

            TokenDictionaryOpen a3 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(a3);

            TokenKeyword a4 = t.GetToken() as TokenKeyword;
            Assert.NotNull(a4);
            Assert.True(a4.Value == ParseKeyword.True);

            TokenName a5 = t.GetToken() as TokenName;
            Assert.NotNull(a5);
            Assert.True(a5.Value == "Name");

            TokenInteger a6 = t.GetToken() as TokenInteger;
            Assert.NotNull(a6);
            Assert.True(a6.Value == 1);

            TokenReal a7 = t.GetToken() as TokenReal;
            Assert.NotNull(a7);
            Assert.True(a7.Value == 3.14f);

            TokenDictionaryClose a8 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(a8);

            TokenArrayClose a9 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(a9);

            TokenComment a10 = t.GetToken() as TokenComment;
            Assert.NotNull(a10);
            Assert.True(a10.Value == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void OneOfEachSpaced()
        {
            Tokenizer t = new Tokenizer(StringToStream(" [  <64 65> << true  /Name  1  3.14 >> ] %comment"));
            t.IgnoreComments = false;

            TokenArrayOpen a1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(a1);

            TokenStringHex a2 = t.GetToken() as TokenStringHex;
            Assert.NotNull(a2);
            Assert.True(a2.Raw == "64 65");

            TokenDictionaryOpen a3 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(a3);

            TokenKeyword a4 = t.GetToken() as TokenKeyword;
            Assert.NotNull(a4);
            Assert.True(a4.Value == ParseKeyword.True);

            TokenName a5 = t.GetToken() as TokenName;
            Assert.NotNull(a5);
            Assert.True(a5.Value == "Name");

            TokenInteger a6 = t.GetToken() as TokenInteger;
            Assert.NotNull(a6);
            Assert.True(a6.Value == 1);

            TokenReal a7 = t.GetToken() as TokenReal;
            Assert.NotNull(a7);
            Assert.True(a7.Value == 3.14f);

            TokenDictionaryClose a8 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(a8);

            TokenArrayClose a9 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(a9);

            TokenComment a10 = t.GetToken() as TokenComment;
            Assert.NotNull(a10);
            Assert.True(a10.Value == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
