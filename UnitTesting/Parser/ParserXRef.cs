using PdfReader;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;
using System.Collections.Generic;

namespace ParserUnitTesting
{
    public class ParserXRef : HelperMethods
    {
        [Fact]
        public void OneSection1()
        {
            Parser p = new Parser(StringToStream("xref\n0 1\n0000000017 00000 n\ntrailer"));
            List<TokenXRefEntry> xrefs = p.ParseXRef();

            Assert.NotNull(xrefs);
            Assert.True(xrefs.Count == 1);
            Assert.True(xrefs[0].Id == 0);
            Assert.True(xrefs[0].Offset == 17);
            Assert.True(xrefs[0].Gen == 0);
        }

        [Fact]
        public void OneSection2()
        {
            Parser p = new Parser(StringToStream("xref\n0 2\n0000000017 00000 n\n0000000166 00001 n\ntrailer"));
            List<TokenXRefEntry> xrefs = p.ParseXRef();

            Assert.NotNull(xrefs);
            Assert.True(xrefs.Count == 2);
            Assert.True(xrefs[0].Id == 0);
            Assert.True(xrefs[0].Offset == 17);
            Assert.True(xrefs[0].Gen == 0);
            Assert.True(xrefs[1].Id == 1);
            Assert.True(xrefs[1].Offset == 166);
            Assert.True(xrefs[1].Gen == 1);
        }

        [Fact]
        public void TwoSection()
        {
            Parser p = new Parser(StringToStream("xref\n0 2\n0000000017 00000 n\n0000000166 00001 n\n\n4 1\n0000027583 65535 n\ntrailer"));
            List<TokenXRefEntry> xrefs = p.ParseXRef();

            Assert.NotNull(xrefs);
            Assert.True(xrefs.Count == 3);
            Assert.True(xrefs[0].Id == 0);
            Assert.True(xrefs[0].Offset == 17);
            Assert.True(xrefs[0].Gen == 0);
            Assert.True(xrefs[1].Id == 1);
            Assert.True(xrefs[1].Offset == 166);
            Assert.True(xrefs[1].Gen == 1);
            Assert.True(xrefs[2].Id == 4);
            Assert.True(xrefs[2].Offset == 27583);
            Assert.True(xrefs[2].Gen == 65535);
        }
    }
}
