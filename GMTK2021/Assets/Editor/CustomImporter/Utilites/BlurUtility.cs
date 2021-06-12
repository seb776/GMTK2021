using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurUtility : MonoBehaviour
{
    private static float avgR = 0;
    private static float avgG = 0;
    private static float avgB = 0;
    private static float blurPixelCount = 0;

    /// <summary>
    /// We use a gaussian blur algorithm to customize the blur on the texture,
    /// according to the radius and iterations number.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="radius"></param>
    /// <param name="iterations"></param>
    public static void ApplyBlur(Texture2D image, int radius, int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            BlurImage(image, radius, true);
            BlurImage(image, radius, false);
        }
    }

    private static void BlurImage(Texture2D image, int blurSize, bool horizontal)
    {

        Texture2D blurred = new Texture2D(image.width, image.height, image.format, true);
        blurred.SetPixels(image.GetPixels());
        int _W = image.width;
        int _H = image.height;
        int xx, yy, x, y;

        if (horizontal)
        {
            for (yy = 0; yy < _H; yy++)
            {
                for (xx = 0; xx < _W; xx++)
                {
                    ResetPixel();

                    //Right side of pixel

                    for (x = xx; (x < xx + blurSize && x < _W); x++)
                    {
                        AddPixel(blurred.GetPixel(x, yy));
                    }

                    //Left side of pixel

                    for (x = xx; (x > xx - blurSize && x > 0); x--)
                    {
                        AddPixel(blurred.GetPixel(x, yy));

                    }


                    CalcPixel();

                    for (x = xx; x < xx + blurSize && x < _W; x++)
                    {
                        image.SetPixel(x, yy, new Color(avgR, avgG, avgB, 1.0f));

                    }
                }
            }
        }

        else
        {
            for (xx = 0; xx < _W; xx++)
            {
                for (yy = 0; yy < _H; yy++)
                {
                    ResetPixel();

                    //Over pixel

                    for (y = yy; (y < yy + blurSize && y < _H); y++)
                    {
                        AddPixel(blurred.GetPixel(xx, y));
                    }
                    //Under pixel

                    for (y = yy; (y > yy - blurSize && y > 0); y--)
                    {
                        AddPixel(blurred.GetPixel(xx, y));
                    }
                    CalcPixel();
                    for (y = yy; y < yy + blurSize && y < _H; y++)
                    {
                        image.SetPixel(xx, y, new Color(avgR, avgG, avgB, 1.0f));

                    }
                }
            }
        }

        image.Apply();
    }

    private static void AddPixel(Color pixel)
    {
        avgR += pixel.r;
        avgG += pixel.g;
        avgB += pixel.b;
        blurPixelCount++;
    }

    private static void ResetPixel()
    {
        avgR = 0.0f;
        avgG = 0.0f;
        avgB = 0.0f;
        blurPixelCount = 0;
    }

    private static void CalcPixel()
    {
        avgR = avgR / blurPixelCount;
        avgG = avgG / blurPixelCount;
        avgB = avgB / blurPixelCount;
    }
}
