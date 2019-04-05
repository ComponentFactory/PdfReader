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
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfNumeric o = i.Obj as PdfNumeric;
            Assert.NotNull(o);
            Assert.True(o.IsInteger);
            Assert.True(o.Integer == 42);
        }

        [Fact]
        public void NumericInteger2()
        {
            Parser p = new Parser(StringToStream("99 0 obj\n42\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 99);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfNumeric o = i.Obj as PdfNumeric;
            Assert.NotNull(o);
            Assert.True(o.IsInteger);
            Assert.True(o.Integer == 42);
        }

        [Fact]
        public void NumericInteger3()
        {
            Parser p = new Parser(StringToStream("1 99 obj\n42 endobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 99);
            Assert.NotNull(i.Obj);

            PdfNumeric o = i.Obj as PdfNumeric;
            Assert.NotNull(o);
            Assert.True(o.IsInteger);
            Assert.True(o.Integer == 42);
        }

        [Fact]
        public void NumericInteger4()
        {
            Parser p = new Parser(StringToStream("101 102 obj 42\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 101);
            Assert.True(i.Gen == 102);
            Assert.NotNull(i.Obj);

            PdfNumeric o = i.Obj as PdfNumeric;
            Assert.NotNull(o);
            Assert.True(o.IsInteger);
            Assert.True(o.Integer == 42);
        }

        [Fact]
        public void StringLiteral()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n(de)\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfString o = i.Obj as PdfString;
            Assert.NotNull(o);
            Assert.True(o.String == "de");
        }

        [Fact]
        public void Name()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n/Example\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfName o = i.Obj as PdfName;
            Assert.NotNull(o);
            Assert.True(o.Name == "Example");
        }

        [Fact]
        public void Array()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n[42]\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfArray o = i.Obj as PdfArray;
            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 1);
            Assert.True(o.Objects[0] is PdfNumeric);
            Assert.True((o.Objects[0] as PdfNumeric).IsInteger);
            Assert.True((o.Objects[0] as PdfNumeric).Integer == 42);
        }

        [Fact]
        public void Dict()
        {
            Parser p = new Parser(StringToStream("1 0 obj\n<</Type (Example)>>\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfDictionary o = i.Obj as PdfDictionary;
            Assert.NotNull(o);
            Assert.True(o.Count == 1);
            Assert.True(o["Type"].Name.Name == "Type");
            Assert.True(o["Type"].Object is PdfString);
            Assert.True((o["Type"].Object as PdfString).String == "Example");
        }
    }
}
