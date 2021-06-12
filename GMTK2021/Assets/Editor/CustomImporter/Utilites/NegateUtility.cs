using UnityEngine;

public class NegateUtility : MonoBehaviour
{
    /// <summary>
    /// We take each pixel and replace each component with 1 - the original value.
    /// </summary>
    /// <param name="image"></param>
    public static void ApplyNegate(Texture2D image)
    {
        for (int i = 0; i < image.width; i++)
        {
            for (int j = 0; j < image.height; j++)
            {
                Color pixel = image.GetPixel(i, j);
                pixel = new Color(1 - pixel.r, 1 - pixel.g, 1 - pixel.b);
                image.SetPixel(i, j, pixel);
            }
        }

        image.Apply();
    }
}
