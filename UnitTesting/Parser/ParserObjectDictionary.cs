using PdfReader;
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

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "de");
        }

        [Fact]
        public void DictStringHex()
        {
            Parser p = new Parser(StringToStream("<</Example <6465>>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "de");
        }

        [Fact]
        public void DictNumericInteger()
        {
            Parser p = new Parser(StringToStream("<</Example 42>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseInteger);
            Assert.True((entry as ParseInteger).Value == 42);
        }

        [Fact]
        public void DictNumericReal()
        {
            Parser p = new Parser(StringToStream("<</Example 3.14>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseReal);
            Assert.True((entry as ParseReal).Value == 3.14f);
        }

        [Fact]
        public void DictNull()
        {
            Parser p = new Parser(StringToStream("<</Example null>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseNull);
        }

        [Fact]
        public void DictName1()
        {
            Parser p = new Parser(StringToStream("<</Example /Other>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseName);
            Assert.True((entry as ParseName).Value == "Other");
        }

        [Fact]
        public void DictName2()
        {
            Parser p = new Parser(StringToStream("<</Example/Other>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseName);
            Assert.True((entry as ParseName).Value == "Other");
        }

        [Fact]
        public void DictBooleanTrue()
        {
            Parser p = new Parser(StringToStream("<</Example true>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseBoolean);
            Assert.True((entry as ParseBoolean).Value);
        }

        [Fact]
        public void DictObjectReference1()
        {
            Parser p = new Parser(StringToStream("<</Example 99 1 R>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseObjectReference);
            Assert.True((entry as ParseObjectReference).Id == 99);
            Assert.True((entry as ParseObjectReference).Gen == 1);
        }

        [Fact]
        public void DictObjectReference2()
        {
            Parser p = new Parser(StringToStream("<</Example 99 1 R /Other 2>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 2);

            ParseObjectBase entry = d["Example"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseObjectReference);
            Assert.True((entry as ParseObjectReference).Id == 99);
            Assert.True((entry as ParseObjectReference).Gen == 1);

            entry = d["Other"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseInteger);
            Assert.True((entry as ParseInteger).Value == 2);
        }

        [Fact]
        public void DictMultiple()
        {
            Parser p = new Parser(StringToStream("<</1 /Example /2 (de) /3 3.14 /4 null>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 4);

            ParseObjectBase entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseName);
            Assert.True((entry as ParseName).Value == "Example");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "de");

            entry = d["3"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseReal);
            Assert.True((entry as ParseReal).Value == 3.14f);

            entry = d["4"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseNull);
        }

        [Fact]
        public void DictMultipleCompact()
        {
            Parser p = new Parser(StringToStream("<</1/Example/2(de)/3 3.14/4 null>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 4);

            ParseObjectBase entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseName);
            Assert.True((entry as ParseName).Value == "Example");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "de");

            entry = d["3"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseReal);
            Assert.True((entry as ParseReal).Value == 3.14f);

            entry = d["4"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseNull);
        }

        [Fact]
        public void DictArray()
        {
            Parser p = new Parser(StringToStream("<</1 [/Example (de) 3.14 null]>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 1);

            ParseObjectBase entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseArray);

            ParseArray o = entry as ParseArray;
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

            ParseObjectBase entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseDictionary);

            d = entry as ParseDictionary;
            Assert.True(d.Count == 1);

            entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "de");
        }

        [Fact]
        public void DictInDict2()
        {
            Parser p = new Parser(StringToStream("<</1 <</A (de)>> /2 <</B (fg)>>>>"));
            ParseDictionary d = p.ParseObject() as ParseDictionary;

            Assert.NotNull(d);
            Assert.True(d.Count == 2);

            ParseObjectBase entry = d["1"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseDictionary);

            ParseDictionary d1 = entry as ParseDictionary;
            Assert.True(d1.Count == 1);

            entry = d1["A"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "de");

            entry = d["2"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseDictionary);

            ParseDictionary d2 = entry as ParseDictionary;
            Assert.True(d2.Count == 1);

            entry = d2["B"];
            Assert.NotNull(entry);
            Assert.True(entry is ParseString);
            Assert.True((entry as ParseString).Value == "fg");
        }
    }
}
