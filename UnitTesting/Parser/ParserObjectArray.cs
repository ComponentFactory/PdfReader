using PdfReader;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace ParserUnitTesting
{
    public class ParserObjectArray : HelperMethods
    {
        [Fact]
        public void ArrayEmpty()
        {
            Parser p = new Parser(StringToStream("[]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 0);
        }

        [Fact]
        public void ArrayName()
        {
            Parser p = new Parser(StringToStream("[/Example]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
        }

        [Fact]
        public void ArrayNumericInteger()
        {
            Parser p = new Parser(StringToStream("[42]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseInteger);
            Assert.True((o.Objects[0] as ParseInteger).Value == 42);
        }

        [Fact]
        public void ArrayNumericReal()
        {
            Parser p = new Parser(StringToStream("[3.14]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseReal);
            Assert.True((o.Objects[0] as ParseReal).Value == 3.14f);
        }

        [Fact]
        public void ArrayStringHex()
        {
            Parser p = new Parser(StringToStream("[<6465>]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseString);
            Assert.True((o.Objects[0] as ParseString).Value == "de");
        }

        [Fact]
        public void ArrayStringLiteral()
        {
            Parser p = new Parser(StringToStream("[(de)]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseString);
            Assert.True((o.Objects[0] as ParseString).Value == "de");
        }

        [Fact]
        public void ArrayBooleanTrue()
        {
            Parser p = new Parser(StringToStream("[true]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseBoolean);
            Assert.True((o.Objects[0] as ParseBoolean).Value);
        }

        [Fact]
        public void ArrayNull()
        {
            Parser p = new Parser(StringToStream("[null]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseNull);
        }

        [Fact]
        public void ArrayObjectReference1()
        {
            Parser p = new Parser(StringToStream("[1 99 R]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseObjectReference);
            Assert.True((o.Objects[0] as ParseObjectReference).Id == 1);
            Assert.True((o.Objects[0] as ParseObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayObjectReference2()
        {
            Parser p = new Parser(StringToStream("[42 1 99 R]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 2);
            Assert.True(o.Objects[0] is ParseInteger);
            Assert.True((o.Objects[0] as ParseInteger).Value == 42);
            Assert.True(o.Objects[1] is ParseObjectReference);
            Assert.True((o.Objects[1] as ParseObjectReference).Id == 1);
            Assert.True((o.Objects[1] as ParseObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayObjectReference3()
        {
            Parser p = new Parser(StringToStream("[42 2 1 99 R]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 3);
            Assert.True(o.Objects[0] is ParseInteger);
            Assert.True((o.Objects[0] as ParseInteger).Value == 42);
            Assert.True(o.Objects[1] is ParseInteger);
            Assert.True((o.Objects[1] as ParseInteger).Value == 2);
            Assert.True(o.Objects[2] is ParseObjectReference);
            Assert.True((o.Objects[2] as ParseObjectReference).Id == 1);
            Assert.True((o.Objects[2] as ParseObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayObjectReference4()
        {
            Parser p = new Parser(StringToStream("[42 2 null 1 99 R]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 4);
            Assert.True(o.Objects[0] is ParseInteger);
            Assert.True((o.Objects[0] as ParseInteger).Value == 42);
            Assert.True(o.Objects[1] is ParseInteger);
            Assert.True((o.Objects[1] as ParseInteger).Value == 2);
            Assert.True(o.Objects[2] is ParseNull);
            Assert.True(o.Objects[3] is ParseObjectReference);
            Assert.True((o.Objects[3] as ParseObjectReference).Id == 1);
            Assert.True((o.Objects[3] as ParseObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayMultiple()
        {
            Parser p = new Parser(StringToStream("[/Example (de) 3.14 1 99 R null]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 5);
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
            Assert.True(o.Objects[1] is ParseString);
            Assert.True((o.Objects[1] as ParseString).Value == "de");
            Assert.True(o.Objects[2] is ParseReal);
            Assert.True((o.Objects[2] as ParseReal).Value == 3.14f);
            Assert.True(o.Objects[3] is ParseObjectReference);
            Assert.True((o.Objects[3] as ParseObjectReference).Id == 1);
            Assert.True((o.Objects[3] as ParseObjectReference).Gen == 99);
            Assert.True(o.Objects[4] is ParseNull);
        }

        [Fact]
        public void ArrayInArray1()
        {
            Parser p = new Parser(StringToStream("[[/Example]]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseArray);

            o = o.Objects[0] as ParseArray;
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
        }

        [Fact]
        public void ArrayInArray2()
        {
            Parser p = new Parser(StringToStream("[[/Example] null]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 2);
            Assert.True(o.Objects[0] is ParseArray);
            Assert.True(o.Objects[1] is ParseNull);

            o = o.Objects[0] as ParseArray;
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
        }

        [Fact]
        public void ArrayInArray3()
        {
            Parser p = new Parser(StringToStream("[null [/Example]]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 2);
            Assert.True(o.Objects[0] is ParseNull);
            Assert.True(o.Objects[1] is ParseArray);

            o = o.Objects[1] as ParseArray;
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
        }

        [Fact]
        public void ArrayInArray4()
        {
            Parser p = new Parser(StringToStream("[null [/Example] 42]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 3);
            Assert.True(o.Objects[0] is ParseNull);
            Assert.True(o.Objects[1] is ParseArray);
            Assert.True(o.Objects[2] is ParseInteger);
            Assert.True((o.Objects[2] as ParseInteger).Value == 42);

            o = o.Objects[1] as ParseArray;
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
        }

        [Fact]
        public void ArrayInArray5()
        {
            Parser p = new Parser(StringToStream("[null [/Example] [null] 42]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 4);
            Assert.True(o.Objects[0] is ParseNull);
            Assert.True(o.Objects[1] is ParseArray);
            Assert.True(o.Objects[2] is ParseArray);
            Assert.True(o.Objects[3] is ParseInteger);
            Assert.True((o.Objects[3] as ParseInteger).Value == 42);

            ParseArray o1 = o.Objects[1] as ParseArray;
            Assert.True(o1.Objects.Count == 1);
            Assert.True(o1.Objects[0] is ParseName);
            Assert.True((o1.Objects[0] as ParseName).Value == "Example");

            ParseArray o2 = o.Objects[2] as ParseArray;
            Assert.True(o2.Objects.Count == 1);
            Assert.True(o2.Objects[0] is ParseNull);
        }

        [Fact]
        public void ArrayDict()
        {
            Parser p = new Parser(StringToStream("[<</Example (de)>>]"));
            ParseArray o = p.ParseObject() as ParseArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is ParseDictionary);

            ParseDictionary d = o.Objects[0] as ParseDictionary;
            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase obj = d["Example"];
            Assert.NotNull(obj);
            Assert.True(obj is ParseString);
            Assert.True((obj as ParseString).Value == "de");
        }
    }
}
