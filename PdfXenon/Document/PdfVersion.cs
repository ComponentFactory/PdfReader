namespace PdfXenon.Standard
{
    public class PdfVersion : PdfObject
    {
        public PdfVersion(PdfDocument doc, int major, int minor)
            : base(doc)
        {
            Major = major;
            Minor = minor;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
    }
}
