using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CroppingUtility : MonoBehaviour
{
    /// <summary>
    /// We use resize, setpixel and getpixel to get all the wanted pixel,
    /// resize the original texture, and set the proper pixels, depending
    /// on the top left point and dimensions.
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="cst"></param>
    public static void ApplyCropping(Texture2D tex, CustomSettings cst)
    {
        int desiredWidth = cst.CropWidth;
        int desiredHeight = cst.CropHeight;
        int desiredX = cst.CropHighX;
        int desiredY = cst.CropHighY;

        Texture2D mTex = new Texture2D(tex.width, tex.height, tex.format, true);
        mTex.SetPixels(tex.GetPixels());
        tex.Resize(desiredWidth, desiredHeight);
        tex.SetPixels(mTex.GetPixels(desiredX, desiredY, desiredWidth, desiredHeight));
        tex.Apply();
    }


}
