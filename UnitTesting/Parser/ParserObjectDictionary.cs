using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace ParserUnitTesting
{
    public class ParserObjectDictionary : HelperMethods
    {
        [Fact]
        public void DictEmpty()
        {
            PdfParser p = new PdfParser(StringToStream("<<>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 0);
        }

        [Fact]
        public void DictStringLiteral()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example (de)>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");
        }

        [Fact]
        public void DictStringHex()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example <6465>>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");
        }

        [Fact]
        public void DictNumericInteger()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example 42>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfNumeric);
            Assert.True((entry.Object as PdfNumeric).IsInteger);
            Assert.True((entry.Object as PdfNumeric).Integer == 42);
        }

        [Fact]
        public void DictNumericReal()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example 3.14>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfNumeric);
            Assert.True((entry.Object as PdfNumeric).IsReal);
            Assert.True((entry.Object as PdfNumeric).Real == 3.14);
        }

        [Fact]
        public void DictNull()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example null>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfNull);
        }

        [Fact]
        public void DictName1()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example /Other>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfName);
            Assert.True((entry.Object as PdfName).Name == "Other");
        }

        [Fact]
        public void DictName2()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example/Other>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfName);
            Assert.True((entry.Object as PdfName).Name == "Other");
        }

        [Fact]
        public void DictBooleanTrue()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example true>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfBoolean);
            Assert.True((entry.Object as PdfBoolean).Value);
        }

        [Fact]
        public void DictObjectReference1()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example 99 1 R>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfObjectReference);
            Assert.True((entry.Object as PdfObjectReference).Id == 99);
            Assert.True((entry.Object as PdfObjectReference).Gen == 1);
        }

        [Fact]
        public void DictObjectReference2()
        {
            PdfParser p = new PdfParser(StringToStream("<</Example 99 1 R /Other 2>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 2);

            PdfDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Example");
            Assert.True(entry.Object is PdfObjectReference);
            Assert.True((entry.Object as PdfObjectReference).Id == 99);
            Assert.True((entry.Object as PdfObjectReference).Gen == 1);

            entry = d["Other"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "Other");
            Assert.True(entry.Object is PdfNumeric);
            Assert.True((entry.Object as PdfNumeric).IsInteger);
            Assert.True((entry.Object as PdfNumeric).Integer == 2);
        }

        [Fact]
        public void DictMultiple()
        {
            PdfParser p = new PdfParser(StringToStream("<</1 /Example /2 (de) /3 3.14 /4 null>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 4);

            PdfDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "1");
            Assert.True(entry.Object is PdfName);
            Assert.True((entry.Object as PdfName).Name == "Example");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "2");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");

            entry = d["3"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "3");
            Assert.True(entry.Object is PdfNumeric);
            Assert.True((entry.Object as PdfNumeric).IsReal);
            Assert.True((entry.Object as PdfNumeric).Real == 3.14);

            entry = d["4"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "4");
            Assert.True(entry.Object is PdfNull);
        }

        [Fact]
        public void DictMultipleCompact()
        {
            PdfParser p = new PdfParser(StringToStream("<</1/Example/2(de)/3 3.14/4 null>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 4);

            PdfDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "1");
            Assert.True(entry.Object is PdfName);
            Assert.True((entry.Object as PdfName).Name == "Example");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "2");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");

            entry = d["3"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "3");
            Assert.True(entry.Object is PdfNumeric);
            Assert.True((entry.Object as PdfNumeric).IsReal);
            Assert.True((entry.Object as PdfNumeric).Real == 3.14);

            entry = d["4"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "4");
            Assert.True(entry.Object is PdfNull);
        }

        [Fact]
        public void DictArray()
        {
            PdfParser p = new PdfParser(StringToStream("<</1 [/Example (de) 3.14 null]>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "1");
            Assert.True(entry.Object is PdfArray);

            PdfArray o = entry.Object as PdfArray;
            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 4);
            Assert.True(o.Objects[0] is PdfName);
            Assert.True((o.Objects[0] as PdfName).Name == "Example");
            Assert.True(o.Objects[1] is PdfString);
            Assert.True((o.Objects[1] as PdfString).String == "de");
            Assert.True(o.Objects[2] is PdfNumeric);
            Assert.True((o.Objects[2] as PdfNumeric).IsReal);
            Assert.True((o.Objects[2] as PdfNumeric).Real == 3.14);
            Assert.True(o.Objects[3] is PdfNull);
        }

        [Fact]
        public void DictInDict1()
        {
            PdfParser p = new PdfParser(StringToStream("<</1 <</1 (de)>>>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            PdfDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "1");
            Assert.True(entry.Object is PdfDictionary);

            d = entry.Object as PdfDictionary;
            Assert.True(d.Count == 1);

            entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "1");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");
        }

        [Fact]
        public void DictInDict2()
        {
            PdfParser p = new PdfParser(StringToStream("<</1 <</A (de)>> /2 <</B (fg)>>>>"));
            PdfDictionary d = p.ParseObject() as PdfDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 2);

            PdfDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "1");
            Assert.True(entry.Object is PdfDictionary);

            PdfDictionary d1 = entry.Object as PdfDictionary;
            Assert.True(d1.Count == 1);

            entry = d1["A"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "A");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "de");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "2");
            Assert.True(entry.Object is PdfDictionary);

            PdfDictionary d2 = entry.Object as PdfDictionary;
            Assert.True(d2.Count == 1);

            entry = d2["B"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Name == "B");
            Assert.True(entry.Object is PdfString);
            Assert.True((entry.Object as PdfString).String == "fg");
        }
    }
}
