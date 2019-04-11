using PdfXenon.Standard;
using System;
using System.Text;
using System.IO;
using Xunit;
using UnitTesting;

namespace TokenizerUnitTesting
{
    public class TokenizerLiteralString : HelperMethods
    {
        [Fact]
        public void LiteralStringLineA()
        {
            Tokenizer t = new Tokenizer(StringToStream("(a)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "a");
            Assert.True(s.ActualString == "a");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringLineB()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc");
            Assert.True(s.ActualString == "abc");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringLineC()
        {
            Tokenizer t = new Tokenizer(StringToStream("  (abc)  "));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 2);
            Assert.True(s.LiteralString == "abc");
            Assert.True(s.ActualString == "abc");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringMultiple()
        {
            Tokenizer t = new Tokenizer(StringToStream("  (a\nb\nc)  "));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 2);
            Assert.True(s.LiteralString == "a\nb\nc");
            Assert.True(s.ActualString == "a\nb\nc");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringContinuation()
        {
            Tokenizer t = new Tokenizer(StringToStream("  (a\\\nb\\\nc)  "));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 2);
            Assert.True(s.LiteralString == "abc");
            Assert.True(s.ActualString == "abc");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringBalanced1()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc()d)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc()d");
            Assert.True(s.ActualString == "abc()d");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringBalanced2()
        {
            Tokenizer t = new Tokenizer(StringToStream("(ab(c)d)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "ab(c)d");
            Assert.True(s.ActualString == "ab(c)d");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringBalanced3()
        {
            Tokenizer t = new Tokenizer(StringToStream("(a(bc)d)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "a(bc)d");
            Assert.True(s.ActualString == "a(bc)d");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringBalanced4()
        {
            Tokenizer t = new Tokenizer(StringToStream("(a(b\\(c)\\)d)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "a(b\\(c)\\)d");
            Assert.True(s.ActualString == "a(b(c))d");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedLineFeed()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\ndef)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\ndef");
            Assert.True(s.ActualString == "abc\ndef");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedCarriageReturn()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\rdef)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\rdef");
            Assert.True(s.ActualString == "abc\rdef");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedTab()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\tdef)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\tdef");
            Assert.True(s.ActualString == "abc\tdef");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedBackspace()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\bdef)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\bdef");
            Assert.True(s.ActualString == "abc\bdef");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedFormFeed()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\fdef)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\fdef");
            Assert.True(s.ActualString == "abc\fdef");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedLeftPara()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\(def)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\(def");
            Assert.True(s.ActualString == "abc(def");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedRightPara()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\)def)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\)def");
            Assert.True(s.ActualString == "abc)def");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedBackslash()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\def)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\def");
            Assert.True(s.ActualString == "abc\\def");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal1()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\100def)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\100def");
            Assert.True(s.ActualString == "abc@def");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal2()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\tdef\\100ghi)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\tdef\\100ghi");
            Assert.True(s.ActualString == "abc\tdef@ghi");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal3()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\100)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\100");
            Assert.True(s.ActualString == "abc@");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal4()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\11)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\11");
            Assert.True(s.ActualString == "abc\t");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal5()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\0)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\0");
            Assert.True(s.ActualString == "abc\0");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal6()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\011)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\011");
            Assert.True(s.ActualString == "abc\t");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void LiteralStringEscapedOctal7()
        {
            Tokenizer t = new Tokenizer(StringToStream("(abc\\0def)"));
            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.LiteralString == "abc\\0def");
            Assert.True(s.ActualString == "abc\0def");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16BigEndianA()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,           // ( 
                                                        0xFE,  0xFF,    // UTF16 Byte Order Mark
                                                        0x00,  0x57,    // W
                                                        0x29            // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "W");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16BigEndianB()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,           // ( 
                                                        0xFE,  0xFF,    // UTF16 Byte Order Mark
                                                        0x20,  0xAC,    // €
                                                        0x29            // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "€");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16BigEndianC()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,                       // ( 
                                                        0xFE,  0xFF,                // UTF16 Byte Order Mark
                                                        0xD8,  0x01, 0xDC,  0x37,   // 𐐷
                                                        0x29                        // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "𐐷");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16BigEndianD()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,                       // ( 
                                                        0xFE,  0xFF,                // UTF16 Byte Order Mark
                                                        0xD8,  0x52, 0xDF,  0x62,   // 𤭢
                                                        0x29                        // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "𤭢");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16BigEndianE()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,                       // ( 
                                                        0xFE,  0xFF,                // UTF16 Byte Order Mark
                                                        0xD8,  0x01, 0xDC,  0x37,   // 𐐷
                                                        0x00,  0x57,                // W
                                                        0x20,  0xAC,                // €
                                                        0xD8,  0x52, 0xDF,  0x62,   // 𤭢
                                                        0x29                        // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "𐐷W€𤭢");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16LittleEndianA()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,           // ( 
                                                        0xFF,  0xFE,    // UTF16 Byte Order Mark
                                                        0x57,  0x00,    // W
                                                        0x29            // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "W");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16LittleEndianB()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,           // ( 
                                                        0xFF,  0xFE,    // UTF16 Byte Order Mark
                                                        0xAC,  0x20,    // €
                                                        0x29            // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "€");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16LittleEndianC()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,                       // ( 
                                                        0xFF,  0xFE,                // UTF16 Byte Order Mark
                                                        0x01,  0xD8, 0x37,  0xDC,   // 𐐷
                                                        0x29                        // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "𐐷");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16LittleEndianD()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,                       // ( 
                                                        0xFF,  0xFE,                // UTF16 Byte Order Mark
                                                        0x52,  0xD8, 0x62,  0xDF,   // 𤭢
                                                        0x29                        // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "𤭢");
            Assert.True(t.GetToken() is TokenEmpty);
        }

        [Fact]
        public void UTF16LittleEndianE()
        {
            Tokenizer t = new Tokenizer(BytesToStream(new byte[] {
                                                        0x28,                       // ( 
                                                        0xFF,  0xFE,                // UTF16 Byte Order Mark
                                                        0x01,  0xD8, 0x37,  0xDC,   // 𐐷
                                                        0x57,  0x00,                // W
                                                        0xAC,  0x20,                // €
                                                        0x52,  0xD8, 0x62,  0xDF,   // 𤭢
                                                        0x29                        // )
                                                      }));

            TokenLiteralString s = t.GetToken() as TokenLiteralString;
            Assert.NotNull(s);
            Assert.True(s.Position == 0);
            Assert.True(s.ActualString == "𐐷W€𤭢");
            Assert.True(t.GetToken() is TokenEmpty);
        }
    }
}
