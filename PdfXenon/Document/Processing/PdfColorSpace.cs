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
                    // Resolve the color space to an object using the resolver
                    PdfObject obj = renderer.Resolver.GetColorSpaceObject(colorSpaceName);
                    if (obj is PdfName name)
                        return FromName(renderer, name.Value);
                    else if (obj is PdfArray array)
                        return FromArray(renderer, array);
                    else
                        throw new NotImplementedException($"Colorspace has unexpected type '{obj.GetType().Name}' when only name and array are recognized.");
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
                case "CalGray":
                    return new PdfColorSpaceCalGray(renderer, array.Objects[1] as PdfDictionary);
                case "CalRGB":
                    return new PdfColorSpaceCalRGB(renderer, array.Objects[1] as PdfDictionary);
                case "ICCBased":
                    {
                        PdfStream stream = renderer.Resolver.GetStream(array.Objects[1] as PdfObjectReference);

                        // The ICCBased stream has an entry 'N' giving the number of color values
                        PdfInteger n = stream.Dictionary.MandatoryValue<PdfInteger>("N");

                        // We ignore the ICC implementation and revert to a matching sized device color space
                        switch (n.Value)
                        {
                            case 1:
                                return new PdfColorSpaceDeviceGray(renderer);
                            case 3:
                                return new PdfColorSpaceDeviceRGB(renderer);
                            case 4:
                                return new PdfColorSpaceDeviceCMYK(renderer);
                            default:
                                throw new NotImplementedException($"Cannot convert from ICCBased color space with '{n.Value}' values to a device color space.");
                        }
                    }
                case "Pattern":
                    return new PdfColorSpacePattern(renderer, FromArray(renderer, (PdfArray)array.Objects[1]));
                case "Separation":
                    return new PdfColorSpaceDeviceGray(renderer);
                default:
                    throw new NotImplementedException($"Colorspace '{dictName}' not implemented.");
            }
        }
    }
}
