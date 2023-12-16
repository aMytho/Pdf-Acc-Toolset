// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
using iText.Kernel.Colors;

namespace Pdf_Acc_Toolset.Services.Util;

public static class ColorUtil
{
    public static float[] ConvertColorToRgb(Color color)
    {
        // If color is already RGB, return it.
        if (color is DeviceRgb)
        {
            return color.GetColorValue();
        }

        // check for CMYK
        if (color is DeviceCmyk)
        {
            return ConvertCmykToRgb(color);
        }
        else
        {
            // Check for gray
            return ConvertGrayToRgb(color);
        }
    }

    private static float[] ConvertCmykToRgb(Color color)
    {
        // Get existing values
        float[] cmykValues = color.GetColorValue();
        float cyan = cmykValues[0];
        float magenta = cmykValues[1];
        float yellow = cmykValues[2];
        float black = cmykValues[3];

        // Convert CMYK to RGB
        float red = 255 * (1 - cyan) * (1 - black);
        float green = 255 * (1 - magenta) * (1 - black);
        float blue = 255 * (1 - yellow) * (1 - black);

        // return result
        return [red, green, blue];
    }

    private static float[] ConvertGrayToRgb(Color color)
    {
        // Get the gray value
        float grayValue = color.GetColorValue()[0];

        // // Convert Gray to RGB
        // float result = (float)Math.Round(255 * (1 - grayValue));
        return [grayValue, grayValue, grayValue];
    }

    public static string ConvertToHex(Color color)
    {
        try {
            float[] rgb = ConvertColorToRgb(color);
            // Get the individual RGB components
            int red = (int)(rgb[0] * 255);
            int green = (int)(rgb[1] * 255);
            int blue = (int)(rgb[2] * 255);

            // Convert each component to hexadecimal format
            return $"#{red:X2}{green:X2}{blue:X2}";
        } catch (Exception) {
            // If anything breaks, show black as the default
            return "#000000";
        }
    }
}