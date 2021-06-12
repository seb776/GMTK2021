using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrayscaleUtility : MonoBehaviour
{
    /// <summary>
    /// We simply apply a grayscale algorithm to every pixel of a texture.
    /// </summary>
    /// <param name="tex"></param>
    public static void ApplyGrayScale(Texture2D tex)
    {
        Color32[] pixels = tex.GetPixels32();
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                Color32 pixel = pixels[x + y * tex.width];
                int p = ((256 * 256 + pixel.r) * 256 + pixel.b) * 256 + pixel.g;
                int b = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int g = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int r = p % 256;
                float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
                Color c = new Color(l, l, l, 1);
                tex.SetPixel(x, y, c);
            }
        }
        tex.Apply();
    }
}
