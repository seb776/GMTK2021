using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Formulae extracted from https://mouaif.wordpress.com/tag/shader/
/// </summary>
public class InputLevelsUtility
{
    public static void ApplyInputLevels(Texture2D tex, float whitepoint, float blackpoint, float gamma)
    {
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                Color pixel = tex.GetPixel(i, j);
                Vector3 color = new Vector3(pixel.r, pixel.g, pixel.b);
                Vector3 colorInterme = LevelsControlInputRange(color, blackpoint, whitepoint);
                Vector3 transition = GammaCorrection(colorInterme, gamma);
                transition = new Vector3(transition.x, transition.y, transition.z);

                pixel = new Color(transition.x, transition.y, transition.z);
                tex.SetPixel(i, j, pixel);
            }
        }

        tex.Apply();
    }

    private static Vector3 LevelsControlInputRange(Vector3 color, float minInput, float maxInput)
    {
        Vector3 max = Vector3.Max(color - new Vector3(minInput, minInput, minInput), new Vector3(0, 0, 0));
        Vector3 equilibrium = new Vector3(maxInput, maxInput, maxInput) - new Vector3(minInput, minInput, minInput);

        Vector3 division = new Vector3(max.x / equilibrium.x, max.y / equilibrium.y, max.z / equilibrium.z);

        return Vector3.Min(division, new Vector3(1, 1, 1));
    }

    private static Vector3 GammaCorrection(Vector3 color, float gamma)
    {
        Vector3 normalGamma = new Vector3(1 / gamma, 1 / gamma, 1 / gamma);
        return new Vector3(Mathf.Pow(color.x, normalGamma.x), Mathf.Pow(color.y, normalGamma.y), Mathf.Pow(color.z, normalGamma.z));
    }
}
