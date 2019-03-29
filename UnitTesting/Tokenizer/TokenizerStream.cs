using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerStream
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
            Assert.True(a1 == TokenBase.ArrayOpen);

            TokenString a2 = t.GetToken() as TokenString;
            Assert.NotNull(a2);
            Assert.True(a2.HexString == "6465");
            Assert.True(a2.IsHex);

            TokenDictionaryOpen a3 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(a3);
            Assert.True(a3 == TokenBase.DictionaryOpen);

            TokenKeyword a4 = t.GetToken() as TokenKeyword;
            Assert.NotNull(a4);
            Assert.True(a4 == TokenKeyword.True);

            TokenName a5 = t.GetToken() as TokenName;
            Assert.NotNull(a5);
            Assert.True(a5.Name == "Name");

            TokenNumeric a6 = t.GetToken() as TokenNumeric;
            Assert.NotNull(a6);
            Assert.True(a6.Integer.HasValue);
            Assert.True(a6.Integer.Value == 1);

            TokenNumeric a7 = t.GetToken() as TokenNumeric;
            Assert.NotNull(a7);
            Assert.True(a7.Real.HasValue);
            Assert.True(a7.Real.Value == 3.14);

            TokenDictionaryClose a8 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(a8);
            Assert.True(a8 == TokenBase.DictionaryClose);

            TokenArrayClose a9 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(a9);
            Assert.True(a9 == TokenBase.ArrayClose);

            TokenComment a10 = t.GetToken() as TokenComment;
            Assert.NotNull(a10);
            Assert.True(a10.Comment == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void OneOfEachSpaced()
        {
            Tokenizer t = new Tokenizer(StringToStream(" [  <64 65> << true  /Name  1  3.14 >> ] %comment"));
            t.IgnoreComments = false;

            TokenArrayOpen a1 = t.GetToken() as TokenArrayOpen;
            Assert.NotNull(a1);
            Assert.True(a1 == TokenBase.ArrayOpen);

            TokenString a2 = t.GetToken() as TokenString;
            Assert.NotNull(a2);
            Assert.True(a2.HexString == "64 65");
            Assert.True(a2.IsHex);

            TokenDictionaryOpen a3 = t.GetToken() as TokenDictionaryOpen;
            Assert.NotNull(a3);
            Assert.True(a3 == TokenBase.DictionaryOpen);

            TokenKeyword a4 = t.GetToken() as TokenKeyword;
            Assert.NotNull(a4);
            Assert.True(a4 == TokenKeyword.True);

            TokenName a5 = t.GetToken() as TokenName;
            Assert.NotNull(a5);
            Assert.True(a5.Name == "Name");

            TokenNumeric a6 = t.GetToken() as TokenNumeric;
            Assert.NotNull(a6);
            Assert.True(a6.Integer.HasValue);
            Assert.True(a6.Integer.Value == 1);

            TokenNumeric a7 = t.GetToken() as TokenNumeric;
            Assert.NotNull(a7);
            Assert.True(a7.Real.HasValue);
            Assert.True(a7.Real.Value == 3.14);

            TokenDictionaryClose a8 = t.GetToken() as TokenDictionaryClose;
            Assert.NotNull(a8);
            Assert.True(a8 == TokenBase.DictionaryClose);

            TokenArrayClose a9 = t.GetToken() as TokenArrayClose;
            Assert.NotNull(a9);
            Assert.True(a9 == TokenBase.ArrayClose);

            TokenComment a10 = t.GetToken() as TokenComment;
            Assert.NotNull(a10);
            Assert.True(a10.Comment == "%comment");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        private MemoryStream StringToStream(string str)
        {
            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(str);
            return new MemoryStream(bytes);
        }
    }
}
