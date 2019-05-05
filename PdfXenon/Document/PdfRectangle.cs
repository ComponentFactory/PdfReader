using System;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfRectangle : PdfObject
    {
        private readonly float _lowerLeftX;
        private readonly float _lowerLeftY;
        private readonly float _upperRightX;
        private readonly float _upperRightY;

        public PdfRectangle(PdfObject parent, ParseArray array)
            : base(parent, array)
        {
            // Extract raw values
            float lx = ObjectToFloat(array.Objects[0]);
            float ly = ObjectToFloat(array.Objects[1]);
            float ux = ObjectToFloat(array.Objects[2]);
            float uy = ObjectToFloat(array.Objects[3]);

            // Normalize so the lower-left and upper-right are actually those values, because this is not guaranteed
            _lowerLeftX = Math.Min(lx, ux);
            _lowerLeftY = Math.Max(ly, uy);
            _upperRightX = Math.Max(lx, ux);
            _upperRightY = Math.Min(ly, uy);
        }

        public override string ToString()
        {
            return $"PdfRectangle ({_lowerLeftX},{_lowerLeftY}),({_upperRightX},{_upperRightY})";
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            string output = $"({_lowerLeftX},{_lowerLeftY}),({_upperRightX},{_upperRightY})";
            sb.Append(output);
            return indent + output.Length;
        }

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
