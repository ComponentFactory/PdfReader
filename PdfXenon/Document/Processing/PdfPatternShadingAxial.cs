using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfPatternShadingAxial : PdfPatternShading
    {
        public PdfPatternShadingAxial(PdfObject parent, PdfDictionary dictionary)
            : base(parent, dictionary)
        {
        }

        public override int ToDebug(StringBuilder sb, int indent)
        {
            base.ToDebug(sb, indent);
            string blank = new string(' ', indent);

            if (Coords != null)
            {
                sb.Append("Coords: ");
                foreach (PdfPoint point in Coords)
                    sb.Append($"{point.ToString()} ");

                sb.Append($"\n{blank}");
            }

            if (Domain != null)
            {
                sb.Append("Domain: ");
                foreach (float item in Domain)
                    sb.Append($"{item.ToString()} ");

                sb.Append($"\n{blank}");
            }
            
            if (Function != null) sb.Append($"Function: {Function}\n{blank}");

            if (Extend != null)
            {
                sb.Append("Extend: ");
                foreach (bool item in Extend)
                    sb.Append($"{item.ToString()} ");

                sb.Append($"\n{blank}");
            }

            return indent;
        }

        public PdfPoint[] Coords
        {
            get
            {
                PdfArray array = Dictionary.MandatoryValue<PdfArray>("Coords");

                float x1 = AsNumber(array.Objects[0]);
                float y1 = AsNumber(array.Objects[1]);
                float x2 = AsNumber(array.Objects[2]);
                float y2 = AsNumber(array.Objects[3]);

                return new PdfPoint[] { new PdfPoint(x1, y1), new PdfPoint(x2, y2) };
            }
        }

        public float[] Domain
        {
            get
            {
                List<float> domain = new List<float>() { 0f, 1f };

                PdfArray array = Dictionary.OptionalValue<PdfArray>("Domain");
                if (array != null)
                {
                    domain.Clear();
                    foreach (PdfObject item in array.Objects)
                        domain.Add(AsNumber(item));
                }

                return domain.ToArray();
            }            
        }

        public string Function
        {
            get
            {
                PdfObject func = Dictionary.MandatoryValueRef<PdfObject>("Function");
                if (func is PdfStream stream)
                {
                    return stream.Value;
                }

                return string.Empty;
            }
        }

        public bool[] Extend
        {
            get
            {
                List<bool> extend = new List<bool>() { false, false };

                PdfArray array = Dictionary.OptionalValue<PdfArray>("Extend");
                if (array != null)
                {
                    extend.Clear();
                    foreach (PdfObject item in array.Objects)
                        extend.Add(AsBoolean(item));
                }

                return extend.ToArray();
            }
        }
    }
}
