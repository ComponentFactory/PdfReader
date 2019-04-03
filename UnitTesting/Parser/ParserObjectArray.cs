using PdfXenon.Standard;
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
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 0);
        }

        [Fact]
        public void ArrayName()
        {
            Parser p = new Parser(StringToStream("[/Example]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
        }

        [Fact]
        public void ArrayNumericInteger()
        {
            Parser p = new Parser(StringToStream("[42]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfNumeric);
            Assert.True((o.Objects[0] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[0] as PdfNumeric).Integer == 42);
        }

        [Fact]
        public void ArrayNumericReal()
        {
            Parser p = new Parser(StringToStream("[3.14]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfNumeric);
            Assert.True((o.Objects[0] as PdfNumeric).IsReal);
            Assert.True((o.Objects[0] as PdfNumeric).Real == 3.14);
        }

        [Fact]
        public void ArrayStringHex()
        {
            Parser p = new Parser(StringToStream("[<6465>]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfString);
            Assert.True((o.Objects[0] as PdfString).String == "de");
        }

        [Fact]
        public void ArrayStringLiteral()
        {
            Parser p = new Parser(StringToStream("[(de)]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfString);
            Assert.True((o.Objects[0] as PdfString).String == "de");
        }

        [Fact]
        public void ArrayBooleanTrue()
        {
            Parser p = new Parser(StringToStream("[true]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfBoolean);
            Assert.True((o.Objects[0] as PdfBoolean).Value);
        }

        [Fact]
        public void ArrayNull()
        {
            Parser p = new Parser(StringToStream("[null]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfNull);
        }

        [Fact]
        public void ArrayObjectReference1()
        {
            Parser p = new Parser(StringToStream("[1 99 R]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfObjectReference);
            Assert.True((o.Objects[0] as PdfObjectReference).Id == 1);
            Assert.True((o.Objects[0] as PdfObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayObjectReference2()
        {
            Parser p = new Parser(StringToStream("[42 1 99 R]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 2);
            Assert.True(o.Objects[0] is PdfNumeric);
            Assert.True((o.Objects[0] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[0] as PdfNumeric).Integer == 42);
            Assert.True(o.Objects[1] is PdfObjectReference);
            Assert.True((o.Objects[1] as PdfObjectReference).Id == 1);
            Assert.True((o.Objects[1] as PdfObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayObjectReference3()
        {
            Parser p = new Parser(StringToStream("[42 2 1 99 R]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 3);
            Assert.True(o.Objects[0] is PdfNumeric);
            Assert.True((o.Objects[0] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[0] as PdfNumeric).Integer == 42);
            Assert.True(o.Objects[1] is PdfNumeric);
            Assert.True((o.Objects[1] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[1] as PdfNumeric).Integer == 2);
            Assert.True(o.Objects[2] is PdfObjectReference);
            Assert.True((o.Objects[2] as PdfObjectReference).Id == 1);
            Assert.True((o.Objects[2] as PdfObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayObjectReference4()
        {
            Parser p = new Parser(StringToStream("[42 2 null 1 99 R]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 4);
            Assert.True(o.Objects[0] is PdfNumeric);
            Assert.True((o.Objects[0] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[0] as PdfNumeric).Integer == 42);
            Assert.True(o.Objects[1] is PdfNumeric);
            Assert.True((o.Objects[1] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[1] as PdfNumeric).Integer == 2);
            Assert.True(o.Objects[2] is PdfNull);
            Assert.True(o.Objects[3] is PdfObjectReference);
            Assert.True((o.Objects[3] as PdfObjectReference).Id == 1);
            Assert.True((o.Objects[3] as PdfObjectReference).Gen == 99);
        }

        [Fact]
        public void ArrayMultiple()
        {
            Parser p = new Parser(StringToStream("[/Example (de) 3.14 1 99 R null]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 5);
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
            Assert.True(o.Objects[1] is PdfString);
            Assert.True((o.Objects[1] as PdfString).String == "de");
            Assert.True(o.Objects[2] is PdfNumeric);
            Assert.True((o.Objects[2] as PdfNumeric).IsReal);
            Assert.True((o.Objects[2] as PdfNumeric).Real == 3.14);
            Assert.True(o.Objects[3] is PdfObjectReference);
            Assert.True((o.Objects[3] as PdfObjectReference).Id == 1);
            Assert.True((o.Objects[3] as PdfObjectReference).Gen == 99);
            Assert.True(o.Objects[4] is PdfNull);
        }

        [Fact]
        public void ArrayInArray1()
        {
            Parser p = new Parser(StringToStream("[[/Example]]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfArray);

            o = o.Objects[0] as PdfArray;
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
        }

        [Fact]
        public void ArrayInArray2()
        {
            Parser p = new Parser(StringToStream("[[/Example] null]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 2);
            Assert.True(o.Objects[0] is PdfArray);
            Assert.True(o.Objects[1] is PdfNull);

            o = o.Objects[0] as PdfArray;
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
        }

        [Fact]
        public void ArrayInArray3()
        {
            Parser p = new Parser(StringToStream("[null [/Example]]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 2);
            Assert.True(o.Objects[0] is PdfNull);
            Assert.True(o.Objects[1] is PdfArray);

            o = o.Objects[1] as PdfArray;
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
        }

        [Fact]
        public void ArrayInArray4()
        {
            Parser p = new Parser(StringToStream("[null [/Example] 42]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 3);
            Assert.True(o.Objects[0] is PdfNull);
            Assert.True(o.Objects[1] is PdfArray);
            Assert.True(o.Objects[2] is PdfNumeric);
            Assert.True((o.Objects[2] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[2] as PdfNumeric).Integer == 42);

            o = o.Objects[1] as PdfArray;
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
        }

        [Fact]
        public void ArrayInArray5()
        {
            Parser p = new Parser(StringToStream("[null [/Example] [null] 42]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 4);
            Assert.True(o.Objects[0] is PdfNull);
            Assert.True(o.Objects[1] is PdfArray);
            Assert.True(o.Objects[2] is PdfArray);
            Assert.True(o.Objects[3] is PdfNumeric);
            Assert.True((o.Objects[3] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[3] as PdfNumeric).Integer == 42);

            PdfArray o1 = o.Objects[1] as PdfArray;
            Assert.True(o1.Objects.Count == 1);
            Assert.True(o1.Objects[0] is PdfName);
            Assert.True((o1.Objects[0] as PdfName).Name == "Example");

            PdfArray o2 = o.Objects[2] as PdfArray;
            Assert.True(o2.Objects.Count == 1);
            Assert.True(o2.Objects[0] is PdfNull);
        }

        [Fact]
        public void ArrayDict()
        {
            Parser p = new Parser(StringToStream("[<</Example (de)>>]"));
            PdfArray o = p.ParseObject() as PdfArray;

            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfDictionary);

            PdfDictionary d = o.Objects[0] as PdfDictionary;
            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");
        }
    }
}
