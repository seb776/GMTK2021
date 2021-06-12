using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// We get the saturation matrix, and apply the saturation depending on the
/// wanted value.
/// </summary>
public class SaturateUtility
{
    public static void ApplySaturation(Texture2D tex, float saturate)
    {
        List<Vector3> matrix = GetSaturationMatrix(saturate);

        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                Color pixel = tex.GetPixel(i, j);
                Vector3 nextCol = MultiplyColorBySaturation(matrix, new Vector3(pixel.r, pixel.g, pixel.b));
                pixel = new Color(nextCol.x, nextCol.y, nextCol.z);
                tex.SetPixel(i,j,pixel);
            }
        }

        tex.Apply();
    }

    private static List<Vector3> GetSaturationMatrix(float saturation)
    {
        List<Vector3> matrixSaturate = new List<Vector3>();
        Vector3 luminance = new Vector3(0.3086f, 0.6094f, 0.0820f);
        float oneMinusSat = 1.0f - saturation;
        Vector3 red = new Vector3(luminance.x * oneMinusSat, luminance.x * oneMinusSat, luminance.x * oneMinusSat);
        red += new Vector3(saturation, 0, 0);

        Vector3 green = new Vector3(luminance.y * oneMinusSat, luminance.y * oneMinusSat, luminance.y * oneMinusSat);
        green += new Vector3(0, saturation, 0);

        Vector3 blue = new Vector3(luminance.z * oneMinusSat, luminance.z * oneMinusSat, luminance.z * oneMinusSat);
        blue += new Vector3(0, 0, saturation);

        matrixSaturate.Add(red);
        matrixSaturate.Add(green);
        matrixSaturate.Add(blue);

        return matrixSaturate;
    }

    private static Vector3 MultiplyColorBySaturation(List<Vector3> matrixSaturate, Vector3 color)
    {
        Vector3 nextCol = new Vector3();
        nextCol.x = (color.x * matrixSaturate[0].x) + (color.y * matrixSaturate[1].x) + (color.z * matrixSaturate[2].x);
        nextCol.y = (color.x * matrixSaturate[0].y) + (color.y * matrixSaturate[1].y) + (color.z * matrixSaturate[2].y);
        nextCol.z = (color.x * matrixSaturate[0].z) + (color.y * matrixSaturate[1].z) + (color.z * matrixSaturate[2].z);
        return nextCol;
    }
}
