using System;

namespace PdfXenon.Standard
{
    public abstract class PdfDecrypt : PdfObject
    {
        public PdfDecrypt(PdfObject parent)
            : base(parent)
        {
        }

        public abstract string DecodeString(PdfString str);
        public abstract byte[] DecodeStringAsBytes(PdfString str);

        public static PdfDecrypt CreateDecrypt(PdfDocument doc, PdfDictionary trailer)
        {
            PdfDecrypt ret = new PdfDecryptNone(doc);

            // Check for optional encryption reference
            PdfObjectReference encryptRef = trailer.OptionalValue<PdfObjectReference>("Encrypt");
            if (encryptRef != null)
            {
                PdfDictionary encryptDict = doc.IndirectObjects.OptionalValue<PdfDictionary>(encryptRef);
                PdfName filter = encryptDict.MandatoryValue<PdfName>("Filter");
                PdfInteger v = encryptDict.OptionalValue<PdfInteger>("V");

                // We only implement the simple Standard, Version 1 scheme
                if ((filter.Value == "Standard") && (v != null) && (v.Value == 1))
                    ret = new PdfDecryptStandard(doc, trailer, encryptDict);

              //  throw new ApplicationException("Can only decrypt the standard handler with version 1.");
            }

            return ret;
        }
    }
}
