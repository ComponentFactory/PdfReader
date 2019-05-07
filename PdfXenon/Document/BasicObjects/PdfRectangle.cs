using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfRectangle : PdfObject
    {
        public PdfRectangle(PdfObject parent, ParseArray array)
            : base(parent, array)
        {
            // Extract raw values
            float lx = ObjectToFloat(array.Objects[0]);
            float ly = ObjectToFloat(array.Objects[1]);
            float ux = ObjectToFloat(array.Objects[2]);
            float uy = ObjectToFloat(array.Objects[3]);

            // Normalize so the lower-left and upper-right are actually those values
            LowerLeft = new PdfPoint(Math.Min(lx, ux), Math.Min(ly, uy));
            UpperRight = new PdfPoint(Math.Max(lx, ux), Math.Max(ly, uy));
        }

        public override string ToString()
        {
            return $"PdfRectangle ({LowerLeft.X},{LowerLeft.Y}),({UpperRight.X},{UpperRight.Y})";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string output = $"({LowerLeft.X},{LowerLeft.Y}),({UpperRight.X},{UpperRight.Y})";
            sb.Append(output);
            return indent + output.Length;
        }

        public PdfPoint LowerLeft { get; private set; }
        public PdfPoint UpperRight { get; private set; }

        public float Width { get { return UpperRight.X - LowerLeft.X; } }
        public float Height { get { return UpperRight.Y - LowerLeft.Y; } }

        private float ObjectToFloat(ParseObject obj)
        {
            // Might be an integer if the value has no fractional part
            if (obj is ParseInteger)
                return (obj as ParseInteger).Value;
            else if (obj is ParseReal)
                return (obj as ParseReal).Value;
            else
                throw new ApplicationException($"Array does not contain numbers that can be converted to a rectangle.");
        }
    }
}
