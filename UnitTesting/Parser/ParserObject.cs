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
            ParseName o = p.ParseObject() as ParseName;

            Assert.NotNull(o);
            Assert.True(o.Value == "Example");
        }

        [Fact]
        public void NumericInteger()
        {
            Parser p = new Parser(StringToStream("42"));
            ParseInteger o = p.ParseObject() as ParseInteger;

            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void NumericReal()
        {
            Parser p = new Parser(StringToStream("3.14"));
            ParseReal o = p.ParseObject() as ParseReal;

            Assert.NotNull(o);
            Assert.True(o.Value == 3.14f);
        }

        [Fact]
        public void StringHex()
        {
            Parser p = new Parser(StringToStream("<6465>"));
            ParseString o = p.ParseObject() as ParseString;

            Assert.NotNull(o);
            Assert.True(o.Value == "de");
        }

        [Fact]
        public void StringLiteral()
        {
            Parser p = new Parser(StringToStream("(de)"));
            ParseString o = p.ParseObject() as ParseString;

            Assert.NotNull(o);
            Assert.True(o.Value == "de");
        }

        [Fact]
        public void BooleanTrue()
        {
            Parser p = new Parser(StringToStream("true"));
            ParseBoolean o = p.ParseObject() as ParseBoolean;

            Assert.NotNull(o);
            Assert.True(o.Value);
        }

        [Fact]
        public void BooleanFalse()
        {
            Parser p = new Parser(StringToStream("false"));
            ParseBoolean o = p.ParseObject() as ParseBoolean;

            Assert.NotNull(o);
            Assert.False(o.Value);
        }

        [Fact]
        public void Null()
        {
            Parser p = new Parser(StringToStream("null"));
            ParseNull o = p.ParseObject() as ParseNull;

            Assert.NotNull(o);
        }

        [Fact]
        public void ObjectReference()
        {
            Parser p = new Parser(StringToStream("2 0 R"));
            ParseObjectReference o = p.ParseObject() as ParseObjectReference;

            Assert.NotNull(o);
            Assert.True(o.Id == 2);
            Assert.True(o.Gen == 0);
        }

    }
}
