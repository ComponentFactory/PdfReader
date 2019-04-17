using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDecryptStandard : PdfDecrypt
    {
        public PdfDecryptStandard(PdfDocument doc, ParseDictionary trailer, ParseDictionary encrypt)
            : base(doc)
        {
            // Extract the first document identifier from the trailer
            ParseArray ids = trailer.MandatoryValue<ParseArray>("ID");
            ParseString id0 = (ParseString)ids.Objects[0];

            // Extract and check the mandatory fields
            ParseInteger R = encrypt.MandatoryValue<ParseInteger>("R");
            ParseInteger length = encrypt.MandatoryValue<ParseInteger>("Length");
            ParseInteger P = encrypt.MandatoryValue<ParseInteger>("P");
            ParseString O = encrypt.MandatoryValue<ParseString>("O");
            ParseString U = encrypt.MandatoryValue<ParseString>("U");

            if (R.Value != 3)
                throw new ApplicationException("Cannot decrypt standard handler with revision other than 3.");

            if ((length.Value < 40) || (length.Value > 128) || (length.Value % 8 != 0))
                throw new ApplicationException("Cannot decrypt with length < 40 or > 128 or not a multiple of 8.");

            byte[] documentId = id0.ValueAsBytes;
            byte[] uBytes = U.ValueAsBytes;
            byte[] oBytes = O.ValueAsBytes;
        }
    }
}
