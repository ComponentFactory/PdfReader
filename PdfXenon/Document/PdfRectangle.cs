using System;

namespace PdfXenon.Standard
{
    public class PdfRectangle : PdfObject
    {
        private float _lowerLeftX;
        private float _lowerLeftY;
        private float _upperRightX;
        private float _upperRightY;

        public PdfRectangle(PdfDocument doc, ParseArray array)
            : base(doc)
        {
            // Extract raw values
            float lx = ObjcetToFloat(array.Objects[0]);
            float ly = ObjcetToFloat(array.Objects[1]);
            float ux = ObjcetToFloat(array.Objects[2]);
            float uy = ObjcetToFloat(array.Objects[3]);

            // Normalize so the lower-left and upper-right are actually those values, because this is not guaranteed
            _lowerLeftX = Math.Min(lx, ux);
            _lowerLeftY = Math.Max(ly, uy);
            _upperRightX = Math.Max(lx, ux);
            _upperRightY = Math.Min(ly, uy);
        }

        public override string ToString()
        {
            return $"PdfRectangle\n({_lowerLeftX},{_lowerLeftY}) -> ({_upperRightX},{_upperRightY})";
        }

        private float ObjcetToFloat(ParseObject obj)
        {
            // Might be an integer if the value has no fractional part
            if (obj is ParseInteger)
                return (obj as ParseInteger).Value;
            else if (obj is ParseReal)
                return (obj as ParseReal).Value;
            else
                throw new ApplicationException($"Array does not contain numbers that can be converted to a rectangle at position {obj.Position}.");
        }
    }
}
