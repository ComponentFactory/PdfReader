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
                    return new PdfDeviceGray(renderer);
                case "DeviceRGB":
                    return new PdfDeviceRGB(renderer);
                case "DeviceCMYK":
                    return new PdfDeviceCMYK(renderer);
                default:
                    {
                        // Get the color space dictionary entry
                        PdfObject obj = renderer.Resolver.GetColorSpaceObject(colorSpaceName);
                        if (obj is PdfName name)
                        {
                            // Can be just a name of the color space to use
                            return FromName(renderer, name.Value);
                        }
                        else if (obj is PdfArray array)
                        {
                            // Must be an array with the first entry the name of the color space
                            colorSpaceName = (array.Objects[0] as PdfName).Value;
                            switch (colorSpaceName)
                            {
                                case "CalGray":
                                    return new PdfCalGray(renderer, array.Objects[1] as PdfDictionary);
                                case "CalRGB":
                                    return new PdfCalRGB(renderer, array.Objects[1] as PdfDictionary);
                                case "Separation":
                                    // Treat the single value as a gray, should be implemented as a tint of a colorspace
                                    return new PdfDeviceGray(renderer);
                                case "ICCBased":
                                    {
                                        PdfStream stream = renderer.Resolver.GetStream(array.Objects[1] as PdfObjectReference);
                                        
                                        // The ICCBased stream has an entry 'N' giving numbers of values in the color
                                        PdfInteger n = stream.Dictionary.MandatoryValue<PdfInteger>("N");

                                        // We ignore the ICC implementation and revert to an matching sized device color space
                                        switch (n.Value)
                                        {
                                            case 1:
                                                return new PdfDeviceGray(renderer);
                                            case 3:
                                                return new PdfDeviceRGB(renderer);
                                            case 4:
                                                return new PdfDeviceCMYK(renderer);
                                        }
                                    }
                                    break;
                            }
                        }

                        throw new NotImplementedException($"Colorspace '{colorSpaceName}' not implemented.");
                    }
            }
        }

        public abstract void ParseColor();
        public abstract PdfRGB ColorAsRGB();
    }
}
