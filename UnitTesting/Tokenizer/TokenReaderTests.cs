using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenReaderTests : HelperMethods
    {
        [Fact]
        public void Single1()
        {
            MemoryStream ms = StringToStream("");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();
            Assert.Null(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 0);
        }

        [Fact]
        public void Single2()
        {
            MemoryStream ms = StringToStream("123");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);
        }

        [Fact]
        public void TwoCarriageReturn()
        {
            MemoryStream ms = StringToStream("123\r456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 4);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }

        [Fact]
        public void TwoLineFeed()
        {
            MemoryStream ms = StringToStream("123\n456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 4);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }

        [Fact]
        public void TwoBoth()
        {
            MemoryStream ms = StringToStream("123\r\n456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 5);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }

        [Fact]
        public void TwoBothReorder()
        {
            MemoryStream ms = StringToStream("123\n\r456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 4);
            Assert.True(splice.Length == 0);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 5);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }

        [Fact]
        public void ThreeCarriageReturn()
        {
            MemoryStream ms = StringToStream("123\r\r456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 4);
            Assert.True(splice.Length == 0);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 5);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }

        [Fact]
        public void ThreeLineFeed()
        {
            MemoryStream ms = StringToStream("123\n\n456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 4);
            Assert.True(splice.Length == 0);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 5);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }

        [Fact]
        public void ThreeBoth()
        {
            MemoryStream ms = StringToStream("123\r\n\r\n456");
            TokenReader r = new TokenReader(ms);
            TokenByteSplice splice = r.ReadLine();

            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 0);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 49);
            Assert.True(splice.Bytes[splice.Start + 1] == 50);
            Assert.True(splice.Bytes[splice.Start + 2] == 51);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 5);
            Assert.True(splice.Length == 0);

            splice = r.ReadLine();
            Assert.NotNull(splice.Bytes);
            Assert.True(splice.Start == 7);
            Assert.True(splice.Length == 3);
            Assert.True(splice.Bytes[splice.Start + 0] == 52);
            Assert.True(splice.Bytes[splice.Start + 1] == 53);
            Assert.True(splice.Bytes[splice.Start + 2] == 54);
        }
    }
}
