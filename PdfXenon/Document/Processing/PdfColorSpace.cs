using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class PdfColorSpace
    {
        public PdfColorSpace(PdfRenderer renderer)
        {
            Renderer = renderer;
        }

        public PdfRenderer Renderer { get; private set; }

        public static PdfColorSpace FromName(PdfRenderer renderer, string colorSpaceName)
        {
            switch (colorSpaceName)
            {
                case "DeviceGray":
                    return new PdfColorSpaceDeviceGray(renderer);
                case "DeviceRGB":
                    return new PdfColorSpaceDeviceRGB(renderer);
                case "DeviceCMYK":
                    return new PdfColorSpaceDeviceCMYK(renderer);
                case "Pattern":
                    return new PdfColorSpacePattern(renderer);
                default:
                    {
                        PdfColorSpace ret = null;

                        // Resolve the color space to an object using the resolver
                        PdfObject obj = renderer.Resolver.GetColorSpaceObject(colorSpaceName);
                        if (obj is PdfName name)
                            ret = FromName(renderer, name.Value);
                        else if (obj is PdfArray array)
                            ret = FromArray(renderer, array);

                        if (ret != null)
                            return ret;

                        throw new NotImplementedException($"Colorspace '{colorSpaceName}' not implemented.");
                    }
            }
        }

        public abstract void ParseColor();
        public abstract PdfColorRGB ColorAsRGB();

        private static PdfColorSpace FromArray(PdfRenderer renderer, PdfArray array)
        {
            // The first entry in the array is the name of the color space
            string dictName = (array.Objects[0] as PdfName).Value;

            switch (dictName)
            {
                case "Pattern":
                    return new PdfColorSpacePattern(renderer, FromArray(renderer, (PdfArray)array.Objects[1]));
                case "CalGray":
                    return new PdfColorSpaceCalGray(renderer, array.Objects[1] as PdfDictionary);
                case "CalRGB":
                    return new PdfColorSpaceCalRGB(renderer, array.Objects[1] as PdfDictionary);
                case "Separation":
                    return new PdfColorSpaceDeviceGray(renderer);
                case "ICCBased":
                    {
                        PdfStream stream = renderer.Resolver.GetStream(array.Objects[1] as PdfObjectReference);

                        // The ICCBased stream has an entry 'N' giving numbers of values in the color
                        PdfInteger n = stream.Dictionary.MandatoryValue<PdfInteger>("N");

                        // We ignore the ICC implementation and revert to an matching sized device color space
                        switch (n.Value)
                        {
                            case 1:
                                return new PdfColorSpaceDeviceGray(renderer);
                            case 3:
                                return new PdfColorSpaceDeviceRGB(renderer);
                            case 4:
                                return new PdfColorSpaceDeviceCMYK(renderer);
                        }
                    }
                    break;
            }

            return null;
        }
    }
}
