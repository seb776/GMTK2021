using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Formulae extracted from https://mouaif.wordpress.com/tag/shader/
/// </summary>
public class OutputLevelsUtility
{
    public static void ApplyOutputLevels(Texture2D tex, float minOutput, float maxOutput)
    {
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                Color pixel = tex.GetPixel(i, j);
                Vector3 color = new Vector3(pixel.r, pixel.g, pixel.b);
                Vector3 maxValue = new Vector3(maxOutput, maxOutput, maxOutput);
                Vector3 minValue = new Vector3(minOutput, minOutput, minOutput);

                Vector3 median = maxValue - minValue;

                color = minValue + new Vector3(median.x * color.x, median.y * color.y, median.z * color.z);
                pixel = new Color(color.x, color.y, color.z);
                tex.SetPixel(i, j, pixel);
            }
        }

        tex.Apply();
    }
}
