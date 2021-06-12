using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightnessUtility
{
    /// <summary>
    /// By iterating on each pixel of the texture, we can apply our brightness settings
    /// to each of them, following the proper formula.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="brightness"></param>
    public static void ApplyBrightness(Texture2D image, float brightness)
    {
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color pixel = image.GetPixel(i, j);

                pixel.r = pixel.r + (brightness / 255);
                pixel.g = pixel.g + (brightness / 255);
                pixel.b = pixel.b + (brightness / 255);

                if (pixel.r < 0)
                    pixel.r = 0;
                if (pixel.r > 1)
                    pixel.r = 1;


                if (pixel.g < 0)
                    pixel.g = 0;
                if (pixel.g > 1)
                    pixel.g = 1;


                if (pixel.b < 0)
                    pixel.b = 0;
                if (pixel.b > 1)
                    pixel.b = 1;

                image.SetPixel(i, j, pixel);
            }
        }

        image.Apply();
    }
}
