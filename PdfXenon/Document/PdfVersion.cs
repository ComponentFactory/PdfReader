namespace PdfXenon.Standard
{
    public class PdfVersion : PdfObject
    {
        public PdfVersion(PdfObject parent, int major, int minor)
            : base(parent)
        {
            Major = major;
            Minor = minor;
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}";
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
    }
}
