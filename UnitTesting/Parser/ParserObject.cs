using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace ParserUnitTesting
{
    public class ParserObject : HelperMethods
    {
        [Fact]
        public void Name()
        {
            Parser p = new Parser(StringToStream("/Example"));
            PdfName o = p.ParseObject() as PdfName;

            Assert.NotNull(o);
            Assert.True(o.Name == "Example");
        }

        [Fact]
        public void NumericInteger()
        {
            Parser p = new Parser(StringToStream("42"));
            PdfNumeric o = p.ParseObject() as PdfNumeric;

            Assert.NotNull(o);
            Assert.True(o.IsInteger);
            Assert.True(o.Integer == 42);
        }

        [Fact]
        public void NumericReal()
        {
            Parser p = new Parser(StringToStream("3.14"));
            PdfNumeric o = p.ParseObject() as PdfNumeric;

            Assert.NotNull(o);
            Assert.True(o.IsReal);
            Assert.True(o.Real == 3.14);
        }

        [Fact]
        public void StringHex()
        {
            Parser p = new Parser(StringToStream("<6465>"));
            PdfString o = p.ParseObject() as PdfString;

            Assert.NotNull(o);
            Assert.True(o.String == "de");
        }

        [Fact]
        public void StringLiteral()
        {
            Parser p = new Parser(StringToStream("(de)"));
            PdfString o = p.ParseObject() as PdfString;

            Assert.NotNull(o);
            Assert.True(o.String == "de");
        }

        [Fact]
        public void BooleanTrue()
        {
            Parser p = new Parser(StringToStream("true"));
            PdfBoolean o = p.ParseObject() as PdfBoolean;

            Assert.NotNull(o);
            Assert.True(o.Value);
        }

        [Fact]
        public void BooleanFalse()
        {
            Parser p = new Parser(StringToStream("false"));
            PdfBoolean o = p.ParseObject() as PdfBoolean;

            Assert.NotNull(o);
            Assert.False(o.Value);
        }

        [Fact]
        public void Null()
        {
            Parser p = new Parser(StringToStream("null"));
            PdfNull o = p.ParseObject() as PdfNull;

            Assert.NotNull(o);
        }

        [Fact]
        public void ObjectReference()
        {
            Parser p = new Parser(StringToStream("2 0 R"));
            PdfObjectReference o = p.ParseObject() as PdfObjectReference;

            Assert.NotNull(o);
            Assert.True(o.Id == 2);
            Assert.True(o.Gen == 0);
        }

    }
}
