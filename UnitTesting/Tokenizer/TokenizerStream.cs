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
    }
}
