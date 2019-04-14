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
            Parser p = new Parser(StringToStream("<<>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 0);
        }

        [Fact]
        public void DictStringLiteral()
        {
            Parser p = new Parser(StringToStream("<</Example (de)>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "de");
        }

        [Fact]
        public void DictStringHex()
        {
            Parser p = new Parser(StringToStream("<</Example <6465>>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "de");
        }

        [Fact]
        public void DictNumericInteger()
        {
            Parser p = new Parser(StringToStream("<</Example 42>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseInteger);
            Assert.True((entry.Object as ParseInteger).Value == 42);
        }

        [Fact]
        public void DictNumericReal()
        {
            Parser p = new Parser(StringToStream("<</Example 3.14>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseReal);
            Assert.True((entry.Object as ParseReal).Value == 3.14f);
        }

        [Fact]
        public void DictNull()
        {
            Parser p = new Parser(StringToStream("<</Example null>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseNull);
        }

        [Fact]
        public void DictName1()
        {
            Parser p = new Parser(StringToStream("<</Example /Other>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseName);
            Assert.True((entry.Object as ParseName).Value == "Other");
        }

        [Fact]
        public void DictName2()
        {
            Parser p = new Parser(StringToStream("<</Example/Other>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseName);
            Assert.True((entry.Object as ParseName).Value == "Other");
        }

        [Fact]
        public void DictBooleanTrue()
        {
            Parser p = new Parser(StringToStream("<</Example true>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseBoolean);
            Assert.True((entry.Object as ParseBoolean).Value);
        }

        [Fact]
        public void DictObjectReference1()
        {
            Parser p = new Parser(StringToStream("<</Example 99 1 R>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseObjectReference);
            Assert.True((entry.Object as ParseObjectReference).Id == 99);
            Assert.True((entry.Object as ParseObjectReference).Gen == 1);
        }

        [Fact]
        public void DictObjectReference2()
        {
            Parser p = new Parser(StringToStream("<</Example 99 1 R /Other 2>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 2);

            ParseDictEntry entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Example");
            Assert.True(entry.Object is ParseObjectReference);
            Assert.True((entry.Object as ParseObjectReference).Id == 99);
            Assert.True((entry.Object as ParseObjectReference).Gen == 1);

            entry = d["Other"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "Other");
            Assert.True(entry.Object is ParseInteger);
            Assert.True((entry.Object as ParseInteger).Value == 2);
        }

        [Fact]
        public void DictMultiple()
        {
            Parser p = new Parser(StringToStream("<</1 /Example /2 (de) /3 3.14 /4 null>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 4);

            ParseDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "1");
            Assert.True(entry.Object is ParseName);
            Assert.True((entry.Object as ParseName).Value == "Example");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "2");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "de");

            entry = d["3"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "3");
            Assert.True(entry.Object is ParseReal);
            Assert.True((entry.Object as ParseReal).Value == 3.14f);

            entry = d["4"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "4");
            Assert.True(entry.Object is ParseNull);
        }

        [Fact]
        public void DictMultipleCompact()
        {
            Parser p = new Parser(StringToStream("<</1/Example/2(de)/3 3.14/4 null>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 4);

            ParseDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "1");
            Assert.True(entry.Object is ParseName);
            Assert.True((entry.Object as ParseName).Value == "Example");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "2");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "de");

            entry = d["3"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "3");
            Assert.True(entry.Object is ParseReal);
            Assert.True((entry.Object as ParseReal).Value == 3.14f);

            entry = d["4"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "4");
            Assert.True(entry.Object is ParseNull);
        }

        [Fact]
        public void DictArray()
        {
            Parser p = new Parser(StringToStream("<</1 [/Example (de) 3.14 null]>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "1");
            Assert.True(entry.Object is ParseArray);

            ParseArray o = entry.Object as ParseArray;
            Assert.NotNull(o);
            Assert.True(o.Objects.Count == 4);
            Assert.True(o.Objects[0] is ParseName);
            Assert.True((o.Objects[0] as ParseName).Value == "Example");
            Assert.True(o.Objects[1] is ParseString);
            Assert.True((o.Objects[1] as ParseString).Value == "de");
            Assert.True(o.Objects[2] is ParseReal);
            Assert.True((o.Objects[2] as ParseReal).Value == 3.14f);
            Assert.True(o.Objects[3] is ParseNull);
        }

        [Fact]
        public void DictInDict1()
        {
            Parser p = new Parser(StringToStream("<</1 <</1 (de)>>>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "1");
            Assert.True(entry.Object is ParseDictionary);

            d = entry.Object as ParseDictionary;
            Assert.True(d.Count == 1);

            entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "1");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "de");
        }

        [Fact]
        public void DictInDict2()
        {
            Parser p = new Parser(StringToStream("<</1 <</A (de)>> /2 <</B (fg)>>>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 2);

            ParseDictEntry entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "1");
            Assert.True(entry.Object is ParseDictionary);

            ParseDictionary d1 = entry.Object as ParseDictionary;
            Assert.True(d1.Count == 1);

            entry = d1["A"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "A");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "de");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "2");
            Assert.True(entry.Object is ParseDictionary);

            ParseDictionary d2 = entry.Object as ParseDictionary;
            Assert.True(d2.Count == 1);

            entry = d2["B"];
            Assert.NotNull(entry);
            Assert.True(entry.Name.Value == "B");
            Assert.True(entry.Object is ParseString);
            Assert.True((entry.Object as ParseString).Value == "fg");
        }
    }
}
