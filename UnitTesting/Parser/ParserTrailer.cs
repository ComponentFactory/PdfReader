using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;
using System.Collections.Generic;

namespace ParserUnitTesting
{
    public class ParserTrailer : HelperMethods
    {
        [Fact]
        public void Trailer()
        {
            Parser p = new Parser(StringToStream("trailer\n<</Size 23/Root 1 0 R/Info 9 0 R/ID[<874BE86964CFD342BA4C701C72F4EB9F><874BE86964CFD342BA4C701C72F4EB9F>] >>\nstartxref"));
            ParseDictionary d = p.ParseTrailer();

            Assert.NotNull(d);
            Assert.True(d.Count == 4);
            Assert.True(d["Size"].Object is ParseInteger);
            Assert.True(d["Root"].Object is ParseObjectReference);
            Assert.True(d["Info"].Object is ParseObjectReference);
            Assert.True(d["ID"].Object is ParseArray);
        }
    }
}
