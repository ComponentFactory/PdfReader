using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerNumeric : HelperMethods
    {
        public float RealPos = +1.79762f;
        public float RealNeg = -1.79762f;

        [Fact]
        public void NumericIntegerZero()
        {
            Tokenizer t = new Tokenizer(StringToStream("0"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerPosition()
        {
            Tokenizer t = new Tokenizer(StringToStream("   0"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 3);
            Assert.True(n.Integer == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerZeroNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-0"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerZeroPositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+0"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerOne()
        {
            Tokenizer t = new Tokenizer(StringToStream("1"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerOneNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-1"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == -1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerOnePositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+1"));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerMax()
        {
            Tokenizer t = new Tokenizer(StringToStream(int.MaxValue.ToString()));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == int.MaxValue);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerMin()
        {
            Tokenizer t = new Tokenizer(StringToStream(int.MinValue.ToString()));
            TokenInteger n = t.GetToken() as TokenInteger;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer == int.MinValue);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealZero()
        {
            Tokenizer t = new Tokenizer(StringToStream("0.0"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealZeroNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-0.0"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealZeroPositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+0.0"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOne()
        {
            Tokenizer t = new Tokenizer(StringToStream("1.0"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOneNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-1.0"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == -1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOnePositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+1.0"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOneDot()
        {
            Tokenizer t = new Tokenizer(StringToStream(".1"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 0.1f);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOneNegativeDot()
        {
            Tokenizer t = new Tokenizer(StringToStream("-.1"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == -0.1f);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOnePositiveDot()
        {
            Tokenizer t = new Tokenizer(StringToStream("+.1"));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == 0.1f);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealDecimals()
        {
            Tokenizer t = new Tokenizer(StringToStream(RealPos.ToString()));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == RealPos);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealDecimalsNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream(RealNeg.ToString()));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == RealNeg);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealDecimalsPositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+" + RealPos.ToString()));
            TokenReal n = t.GetToken() as TokenReal;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real == RealPos);
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
