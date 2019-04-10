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
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfStream o = i.Obj as PdfStream;
            Assert.NotNull(o);
            Assert.True(o.StringContent == "de");
        }

        [Fact]
        public void Unfiltered2()
        {
            Parser p = new Parser(StringToStream("1 0 obj<</Length 2>>stream\rde\nendstream\nendobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 1);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfStream o = i.Obj as PdfStream;
            Assert.NotNull(o);
            Assert.True(o.StringContent == "de");
        }

        [Fact]
        public void FlateDecode()
        {
            Parser p = new Parser(StringBytesToStream("4 0 obj <</Filter /FlateDecode /Length 136>>\nstream\n",
                                                      new byte[] { 0x78, 0x9c, 0x2d, 0x8c, 0x31,
                                                                   0x0b, 0xc2, 0x30, 0x14, 0x06, 0xf7, 0x07, 0xef, 0x3f, 0x7c, 0xa3, 0x3a, 0x24, 0x2f, 0x31, 0x35,
                                                                   0x09, 0x94, 0x0e, 0x4d, 0x6b, 0x51, 0x28, 0x28, 0x06, 0x1c, 0xc4, 0x51, 0x3b, 0x29, 0xa8, 0xff,
                                                                   0x1f, 0x4c, 0xc5, 0xdb, 0x0e, 0x8e, 0x83, 0x3e, 0xa0, 0xae, 0xf5, 0x98, 0x76, 0x1d, 0xa4, 0x69, 0xd0, 0x76,
                                                                   0x09, 0x2f, 0x26, 0x51, 0x32, 0x13, 0x82, 0x37, 0x10, 0x54, 0xb1, 0x52, 0x6b, 0x8b, 0xe0, 0x8c,
                                                                   0x8a, 0x16, 0xef, 0x1b, 0xd3, 0x79, 0x85, 0x27, 0x53, 0x9b, 0x99, 0xf4, 0xd6, 0xc0, 0x18, 0x25,
                                                                   0x0e, 0xf9, 0xce, 0x34, 0xd7, 0x02, 0x03, 0x6f, 0x95, 0x58, 0x07, 0x5f, 0x45, 0xe5, 0x36, 0xc8, 0x8f, 0xd2,
                                                                   0x0d, 0x27, 0x8f, 0xe9, 0x53, 0xd6, 0x98, 0x7e, 0x16, 0xfe, 0x36, 0x30, 0x5d, 0x16, 0x58, 0x5e,
                                                                   0x91, 0xf7, 0x4c, 0x79, 0x39, 0x1e, 0x99, 0xd0, 0x8f, 0x90, 0x5f, 0x51, 0xd1, 0x1e, 0x1d },
                                                      "endstream\n" +
                                                      "endobj"));
            PdfIndirectObject i = p.ParseIndirectObject() as PdfIndirectObject;

            Assert.NotNull(i);
            Assert.True(i.Id == 4);
            Assert.True(i.Gen == 0);
            Assert.NotNull(i.Obj);

            PdfStream o = i.Obj as PdfStream;
            Assert.NotNull(o);
            // TODO
        }
    }
}
