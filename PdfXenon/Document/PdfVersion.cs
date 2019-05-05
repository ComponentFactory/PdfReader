using System.Text;

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
            return $"PdfVersion Version:{Major}.{Minor}";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string output = $"{Major}.{Minor}";
            sb.Append(output);
            return indent + output.Length;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }
    }
}
