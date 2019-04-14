using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace ParserUnitTesting
{
    public class ParserIndirectObject : HelperMethods
    {
        [Fact]
        public void NumericInteger1()
        {
            Parser p = new Parser(StringToStream("1 0 obj 42 endobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseInteger o = i.Object as ParseInteger;
            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void NumericInteger2()
        {
            Parser p = new Parser(StringToStream("99 0 obj\n42\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 99);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseInteger o = i.Object as ParseInteger;
            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void NumericInteger3()
        {
            Parser p = new Parser(StringToStream("1 99 obj\n42 endobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 99);
            Assert.NotNull(i.Object);

            ParseInteger o = i.Object as ParseInteger;
            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void NumericInteger4()
        {
            Parser p = new Parser(StringToStream("101 102 obj 42\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 101);
            Assert.True(i.Gen == 102);
            Assert.NotNull(i.Object);

            ParseInteger o = i.Object as ParseInteger;
            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void NumericInteger5()
        {
            Parser p = new Parser(StringToStream("1 0 obj42 endobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseInteger o = i.Object as ParseInteger;
            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void NumericInteger6()
        {
            Parser p = new Parser(StringToStream("1 0 obj42endobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseInteger o = i.Object as ParseInteger;
            Assert.NotNull(o);
            Assert.True(o.Value == 42);
        }

        [Fact]
        public void StringLiteral()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n(de)\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseString o = i.Object as ParseString;
            Assert.NotNull(o);
            Assert.True(o.Value == "de");
        }

        [Fact]
        public void Name()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n/Example\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseName o = i.Object as ParseName;
            Assert.NotNull(o);
            Assert.True(o.Value == "Example");
        }

        [Fact]
        public void Array()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n[42]\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseArray o = i.Object as ParseArray;
            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseInteger);
            Assert.True((o.Objects[0] as ParseInteger).Value == 42);
        }

        [Fact]
        public void Dict()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n<</Type (Example)>>\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Object);

            ParseDictionary o = i.Object as ParseDictionary;
            Assert.NotNull(o);
            Assert.True(o.Count == 1);
            Assert.True(o["Type"].Name.Value == "Type");
            Assert.True(o["Type"].Object is ParseString);
            Assert.True((o["Type"].Object as ParseString).Value == "Example");
        }
    }
}
