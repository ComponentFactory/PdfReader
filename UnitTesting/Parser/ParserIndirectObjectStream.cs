using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace ParserUnitTesting
{
    public class ParserIndirectObjectStream : HelperMethods
    {
        [Fact]
        public void Unfiltered1()
        {
            Parser p = new Parser(StringToStream("1 0 obj<</Length 2>>stream\n\rdeendstream\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Child);

            ParseStream o = i.Child as ParseStream;
            Assert.NotNull(o);
            Assert.True(o.ContentAsString == "de");
        }

        [Fact]
        public void Unfiltered2()
        {
            Parser p = new Parser(StringToStream("1 0 obj<</Length 2>>stream\rde\nendstream\nendobj"));
            ParseIndirectObject i = p.ParseIndirectObject() as ParseIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Child);

            ParseStream o = i.Child as ParseStream;
            Assert.NotNull(o);
            Assert.True(o.ContentAsString == "de");
        }
    }
}
