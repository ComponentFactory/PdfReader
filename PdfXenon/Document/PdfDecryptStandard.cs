using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDecryptStandard : PdfDecrypt
    {
        public PdfDecryptStandard(PdfObject parent, PdfDictionary trailer, PdfDictionary encrypt)
            : base(parent)
        {
            // Extract the first document identifier from the trailer
            PdfArray ids = trailer.MandatoryValue<PdfArray>("ID");
            PdfString id0 = (PdfString)ids.Objects[0];

            // Extract and check the mandatory fields
            PdfInteger R = encrypt.MandatoryValue<PdfInteger>("R");
            PdfInteger length = encrypt.MandatoryValue<PdfInteger>("Length");
            PdfInteger P = encrypt.MandatoryValue<PdfInteger>("P");
            PdfString O = encrypt.MandatoryValue<PdfString>("O");
            PdfString U = encrypt.MandatoryValue<PdfString>("U");

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
