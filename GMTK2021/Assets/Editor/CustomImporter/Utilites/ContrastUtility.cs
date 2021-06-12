using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContrastUtility
{
    /// <summary>
    /// We use the proper contrast algorithm to adapt the contrast depending
    /// on the selected value.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="contrast"></param>
    public static void ApplyContrast(Texture2D image, double contrast)
    {

        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color pixel = image.GetPixel(i, j);

                pixel.r = (float)((pixel.r - 0.5f) * contrast) + 0.5f;
                pixel.g = (float)((pixel.g - 0.5f) * contrast) + 0.5f;
                pixel.b = (float)((pixel.b - 0.5f) * contrast) + 0.5f;

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
