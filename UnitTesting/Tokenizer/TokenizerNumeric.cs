using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;

namespace UnitTesting
{
    public class TokenizerNumeric : HelperMethods
    {
        public double RealPos = +1.79769313486232;
        public double RealNeg = -1.79769313486232;

        [Fact]
        public void NumericIntegerZero()
        {
            Tokenizer t = new Tokenizer(StringToStream("0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerPosition()
        {
            Tokenizer t = new Tokenizer(StringToStream("   0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 3);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerZeroNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerZeroPositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerOne()
        {
            Tokenizer t = new Tokenizer(StringToStream("1"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerOneNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-1"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == -1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerOnePositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+1"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerMax()
        {
            Tokenizer t = new Tokenizer(StringToStream(int.MaxValue.ToString()));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == int.MaxValue);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericIntegerMin()
        {
            Tokenizer t = new Tokenizer(StringToStream(int.MinValue.ToString()));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Integer.HasValue);
            Assert.True(n.Integer.Value == int.MinValue);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealZero()
        {
            Tokenizer t = new Tokenizer(StringToStream("0.0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealZeroNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-0.0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealZeroPositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+0.0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 0);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOne()
        {
            Tokenizer t = new Tokenizer(StringToStream("1.0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOneNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream("-1.0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == -1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOnePositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+1.0"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOneDot()
        {
            Tokenizer t = new Tokenizer(StringToStream(".1"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 0.1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOneNegativeDot()
        {
            Tokenizer t = new Tokenizer(StringToStream("-.1"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == -0.1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealOnePositiveDot()
        {
            Tokenizer t = new Tokenizer(StringToStream("+.1"));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == 0.1);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealDecimals()
        {
            Tokenizer t = new Tokenizer(StringToStream(RealPos.ToString()));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == RealPos);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealDecimalsNegative()
        {
            Tokenizer t = new Tokenizer(StringToStream(RealNeg.ToString()));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == RealNeg);
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void NumericRealDecimalsPositive()
        {
            Tokenizer t = new Tokenizer(StringToStream("+" + RealPos.ToString()));
            TokenNumeric n = t.GetToken() as TokenNumeric;
            Assert.NotNull(n);
            Assert.True(n.Position == 0);
            Assert.True(n.Real.HasValue);
            Assert.True(n.Real.Value == RealPos);
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
