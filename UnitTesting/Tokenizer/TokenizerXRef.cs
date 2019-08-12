using PdfReader;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerXRefOffset : HelperMethods
    {
        [Fact]
        public void Exact1()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("9\n%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 9); 
        }

        [Fact]
        public void Exact2()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("89\n%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 89);
        }

        [Fact]
        public void Exact5()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("12345\n%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 12345);
        }

        [Fact]
        public void Exact8()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("12345678\n%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 12345678);
        }

        [Fact]
        public void Exact12()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("123456789000\n%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 123456789000);
        }

        [Fact]
        public void Whitespace1()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("9\r \t\n%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 9);
        }

        [Fact]
        public void Whitespace2()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("9\r \t\n%%EOF   \t\r\n", 1024));
            Assert.True(t.GetXRefOffset() == 9);
        }

        [Fact]
        public void Whitespace3()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("9%%EOF", 1024));
            Assert.True(t.GetXRefOffset() == 9);
        }

        [Fact]
        public void IgnoredComment()
        {
            Tokenizer t = new Tokenizer(StringPrePaddedToStream("9%%EOF%EOFEOFF", 1024));
            Assert.True(t.GetXRefOffset() == 9);
        }
    }
}
