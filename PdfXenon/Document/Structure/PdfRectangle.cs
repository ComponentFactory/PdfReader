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
            LowerLeftX = Math.Min(lx, ux);
            LowerLeftY = Math.Min(ly, uy);
            UpperRightX = Math.Max(lx, ux);
            UpperRightY = Math.Max(ly, uy);
        }

        public override string ToString()
        {
            return $"({LowerLeftX},{LowerLeftY}) -> ({UpperRightX},{UpperRightY})";
        }

        public override void Visit(IPdfObjectVisitor visitor)
        {
            visitor.Visit(this);
        }

        public float LowerLeftX { get; private set; }
        public float LowerLeftY { get; private set; }
        public float UpperRightX { get; private set; }
        public float UpperRightY { get; private set; }

        public float Width { get { return UpperRightX - LowerLeftX; } }
        public float Height { get { return UpperRightY - LowerLeftY; } }

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
