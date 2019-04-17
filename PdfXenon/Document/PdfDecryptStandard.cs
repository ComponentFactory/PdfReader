using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDecryptStandard : PdfDecrypt
    {
        public PdfDecryptStandard(PdfDocument doc, ParseDictionary parse, byte[] documentId)
            : base(doc)
        {
            ParseInteger R = parse.MandatoryValue<ParseInteger>("R");
            if (R.Value != 3)
                throw new ApplicationException("Cannot decrypt standard handler with revision other than 3.");

            ParseInteger length = parse.MandatoryValue<ParseInteger>("Length");
            if ((length.Value < 40) || (length.Value > 128) || (length.Value % 8 != 0))
                throw new ApplicationException("Cannot decrypt with length < 40 or > 128 or not a multiple of 8.");

            ParseInteger P = parse.MandatoryValue<ParseInteger>("P");
            ParseString O = parse.MandatoryValue<ParseString>("O");
            ParseString U = parse.MandatoryValue<ParseString>("U");

            byte[] uBytes = U.ValueAsBytes;
            byte[] oBytes = O.ValueAsBytes;
        }
    }
}
