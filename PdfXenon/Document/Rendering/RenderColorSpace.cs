using System;
using System.Collections.Generic;
using System.Text;

namespace PdfXenon.Standard
{
    public abstract class RenderColorSpace : RenderObject
    {
        public RenderColorSpace(RenderObject parent)
            : base(parent)
        {
        }

        public abstract void ParseParameters();

        public static RenderColorSpace FromName(RenderObject parent, string colorSpaceName)
        {
            switch (colorSpaceName)
            {
                case "DeviceGray":
                    return new RenderColorSpaceDeviceGray(parent);
                case "DeviceRGB":
                    return new RenderColorSpaceDeviceRGB(parent);
                case "DeviceCMYK":
                    return new RenderColorSpaceDeviceCMYK(parent);
                case "Pattern":
                    return new RenderColorSpacePattern(parent);
                default:
                    // Must be a name that needs looking up in the color space dictionary, use resolver
                    PdfObject obj = parent.Renderer.Resolver.GetColorSpaceObject(colorSpaceName);
                    if (obj is PdfName name)
                        return FromName(parent, name.Value);
                    else if (obj is PdfArray array)
                        return FromArray(parent, array);
                    else
                        throw new NotImplementedException($"Colorspace has unexpected type '{obj.GetType().Name}' when only name and array are recognized.");
            }
        }

        private static RenderColorSpace FromArray(RenderObject parent, PdfArray array)
        {
            // The first entry in the array is the name of the color space
            string dictName = (array.Objects[0] as PdfName).Value;

            switch (dictName)
            {
                case "CalGray":
                    return new RenderColorSpaceCalGray(parent, array.Objects[1] as PdfDictionary);
                case "CalRGB":
                    return new RenderColorSpaceCalRGB(parent, array.Objects[1] as PdfDictionary);
                case "ICCBased":
                    {
                        // Second entry in array is a reference to a stream, resolve it
                        PdfStream stream = parent.Renderer.Resolver.GetStream(array.Objects[1] as PdfObjectReference);

                        // The ICCBased stream has an entry 'N' giving the number of color values
                        PdfInteger n = stream.Dictionary.MandatoryValue<PdfInteger>("N");

                        // We ignore the ICC implementation and revert to a matching sized device color space
                        switch (n.Value)
                        {
                            case 1:
                                return new RenderColorSpaceDeviceGray(parent);
                            case 3:
                                return new RenderColorSpaceDeviceRGB(parent);
                            case 4:
                                return new RenderColorSpaceDeviceCMYK(parent);
                            default:
                                throw new NotImplementedException($"Cannot convert from ICCBased color space with '{n.Value}' components to a device color space.");
                        }
                    }
                case "Pattern":
                    return new RenderColorSpacePattern(parent);
                default:
                    throw new NotImplementedException($"Colorspace '{dictName}' not implemented.");
            }
        }
    }
}
