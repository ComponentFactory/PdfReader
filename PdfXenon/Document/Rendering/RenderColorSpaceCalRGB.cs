using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public class RenderColorSpaceCalRGB : RenderColorSpaceRGB
    {
        private Matrix31 _abc = new Matrix31(0f, 0f, 0f);
        private Matrix31 _whitePoint = new Matrix31(0f, 1f, 0f);
        private Matrix31 _blackPoint = new Matrix31(0f, 0f, 0f);
        private Matrix31 _gamma = new Matrix31(1f, 1f, 1f);
        private Matrix33 _matrix = new Matrix33(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);
        private Matrix33 _xyz_rgb;

        private static readonly Matrix33 _rgb_xyz;
        private static readonly Matrix33 _rgb_xyz_inverse;    
        private static readonly byte[] _sRGB_Samples1 = new byte[] {   0,   3,   6,  10,  13,  15,  18,  20,  22,  23,  25,  27,  28,  30,  31,  32,
                                                                      34,  35,  36,  37,  38,  39,  40,  41,  42,  43,  44,  45,  46,  47,  48,  49,
                                                                      49,  50,  51,  52,  53,  53,  54,  55,  56,  56,  57,  58,  58,  59,  60,  61,
                                                                      61,  62,  62,  63,  64,  64,  65,  66,  66,  67,  67,  68,  68,  69,  70,  70,
                                                                      71,  71,  72,  72,  73,  73,  74,  74,  75,  76,  76,  77,  77,  78,  78,  79,
                                                                      79,  79,  80,  80,  81,  81,  82,  82,  83,  83,  84,  84,  85,  85,  85,  86,
                                                                      86,  87,  87,  88,  88,  88,  89,  89,  90,  90,  91,  91,  91,  92,  92,  93,
                                                                      93,  93,  94,  94,  95,  95,  95,  96,  96,  97,  97,  97,  98,  98,  98,  99,
                                                                      99,  99, 100, 100, 101, 101, 101, 102, 102, 102, 103, 103, 103, 104, 104, 104,
                                                                     105, 105, 106, 106, 106, 107, 107, 107, 108, 108, 108, 109, 109, 109, 110, 110,
                                                                     110, 110, 111, 111, 111, 112, 112, 112, 113, 113, 113, 114, 114, 114, 115, 115,
                                                                     115, 115, 116, 116, 116, 117, 117, 117, 118, 118, 118, 118, 119, 119, 119, 120 };

        private static readonly byte[] _sRGB_Samples2 = new byte[] { 120, 121, 122, 124, 125, 126, 127, 128, 129, 130, 131, 132, 133, 134, 135, 136,
                                                                     137, 138, 139, 140, 141, 142, 143, 144, 145, 146, 147, 148, 148, 149, 150, 151,
                                                                     152, 153, 154, 155, 155, 156, 157, 158, 159, 159, 160, 161, 162, 163, 163, 164,
                                                                     165, 166, 167, 167, 168, 169, 170, 170, 171, 172, 173, 173, 174, 175, 175, 176,
                                                                     177, 178, 178, 179, 180, 180, 181, 182, 182, 183, 184, 185, 185, 186, 187, 187,
                                                                     188, 189, 189, 190, 190, 191, 192, 192, 193, 194, 194, 195, 196, 196, 197, 197,
                                                                     198, 199, 199, 200, 200, 201, 202, 202, 203, 203, 204, 205, 205, 206, 206, 207,
                                                                     208, 208, 209, 209, 210, 210, 211, 212, 212, 213, 213, 214, 214, 215, 215, 216,
                                                                     216, 217, 218, 218, 219, 219, 220, 220, 221, 221, 222, 222, 223, 223, 224, 224,
                                                                     225, 226, 226, 227, 227, 228, 228, 229, 229, 230, 230, 231, 231, 232, 232, 233,
                                                                     233, 234, 234, 235, 235, 236, 236, 237, 237, 238, 238, 238, 239, 239, 240, 240,
                                                                     241, 241, 242, 242, 243, 243, 244, 244, 245, 245, 246, 246, 246, 247, 247, 248,
                                                                     248, 249, 249, 250, 250, 251, 251, 251, 252, 252, 253, 253, 254, 254, 255, 255 };

        static RenderColorSpaceCalRGB()
        {
            // We use the sRGB (Standard RGB color space as defined by HP/Microsoft)
            _rgb_xyz = new Matrix33(0.64f, 0.30f, 0.15f, 
                                    0.33f, 0.60f, 0.06f, 
                                    0.03f, 0.10f, 0.79f);

            _rgb_xyz_inverse = _rgb_xyz.Inverse();
        }

        public RenderColorSpaceCalRGB(RenderObject parent, PdfDictionary dictionary)
            : base(parent)
        {
            PdfArray array = dictionary.MandatoryValue<PdfArray>("WhitePoint");
            for (int i = 0; i < 3; i++)
                _whitePoint[i] = array.Objects[i].AsNumber();

            array = dictionary.OptionalValue<PdfArray>("BlackPoint");
            if (array != null)
            {
                for (int i = 0; i < 3; i++)
                    _blackPoint[i] = array.Objects[i].AsNumber();
            }

            array = dictionary.OptionalValue<PdfArray>("Gamma");
            if (array != null)
            {
                for (int i = 0; i < 3; i++)
                    _gamma[i] = array.Objects[i].AsNumber();
            }

            array = dictionary.OptionalValue<PdfArray>("Matrix");
            if (array != null)
            {
                for(int i = 0; i < 9; i++)
                    _matrix[i] = array.Objects[i].AsNumber();
            }

            // Find the XYZ -> RGB conversion matrix based on the provided whitepoint
            Matrix31 wp_inverse = _rgb_xyz_inverse.Transform(_whitePoint);
            Matrix33 wp_inverse_rgb = new Matrix33(wp_inverse[0], 0, 0, 0, wp_inverse[1], 0, 0, 0, wp_inverse[2]);
            _xyz_rgb = _rgb_xyz.Multiply(wp_inverse_rgb).Inverse();
        }

        public override int NumberOfComponents()
        {
            return 3;
        }

        public override void Parse(float[] values)
        {
            _abc[0] = values[0];
            _abc[1] = values[1];
            _abc[2] = values[2];
        }

        public override void ParseParameters()
        {
            float[] values = new float[3];
            values[2] = Renderer.OperandAsNumber();
            values[1] = Renderer.OperandAsNumber();
            values[0] = Renderer.OperandAsNumber();
            Parse(values);
        }

        public override RenderColorRGB GetColorRGB()
        {
            // Convert from ABC -> XYZ
            Matrix31 xyz = new Matrix31((float)((_matrix[0] * Math.Pow(_abc[0], _gamma[0])) + (_matrix[3] * Math.Pow(_abc[1], _gamma[1])) + (_matrix[6] * Math.Pow(_abc[2], _gamma[2]))),
                                        (float)((_matrix[1] * Math.Pow(_abc[0], _gamma[0])) + (_matrix[4] * Math.Pow(_abc[1], _gamma[1])) + (_matrix[7] * Math.Pow(_abc[2], _gamma[2]))),
                                        (float)((_matrix[2] * Math.Pow(_abc[0], _gamma[0])) + (_matrix[5] * Math.Pow(_abc[1], _gamma[1])) + (_matrix[8] * Math.Pow(_abc[2], _gamma[2]))));

            // Convert from XYZ -> RGB
            Matrix31 rgb = _xyz_rgb.Transform(xyz);

            return new RenderColorRGB(RGBSampling(rgb[0]),
                                   RGBSampling(rgb[1]),
                                   RGBSampling(rgb[2]));
        }

        private float RGBSampling(float c)
        {
            // Range check 0.0 -> 1.0
            c = Math.Min(1, Math.Max(0, c));

            int scale = (int)(c * 1023);
            if (scale < 0)
                scale = 0;

            if (scale < 192)
                c = (_sRGB_Samples1[scale] / 255.0f);
            else
                c = (_sRGB_Samples2[scale / 4 - 48] / 255.0f);

            return c;
        }
    }
}
