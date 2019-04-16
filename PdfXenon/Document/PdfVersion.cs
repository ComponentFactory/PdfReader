namespace PdfXenon.Standard
{
    public class PdfVersion : PdfObject
    {
        public PdfVersion(PdfDocument doc)
            : base(doc)
        {
        }

        public int Major { get; set; }
        public int Minor { get; set; }
    }
}
