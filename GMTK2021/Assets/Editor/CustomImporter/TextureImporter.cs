using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureImporter : AssetPostprocessor
{
    // We use the onpostprocesstexture function from the AssetPostprocessor class
    // To handle the modifications to our texture.
    private void OnPostprocessTexture(Texture2D texture)
    {
        // We try to get a .opimport file with the asset name, at the asset path.
        string path = (Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath)).Replace("\\", "/") + ".opimport";
        if (!File.Exists(path))
        {
            Debug.Log(path + " not found. No custom settings.");
            return;
        }

        // If found, we convert the text in the file in a CustomSettings.
        CustomSettings cst = JsonUtility.FromJson<CustomSettings>(File.ReadAllText(path));

        // Depending on the data in the custom settings object, we apply different operation
        if (cst.GreyScale)
            GrayscaleUtility.ApplyGrayScale(texture);

        if (cst.CropSize)
            CroppingUtility.ApplyCropping(texture, cst);

        if (cst.Blur)
            BlurUtility.ApplyBlur(texture, cst.radius, cst.iterations);

        if (cst.Negate)
            NegateUtility.ApplyNegate(texture);

        if (cst.Brightness)
            BrightnessUtility.ApplyBrightness(texture, cst.brightval);

        if (cst.Contrast)
            ContrastUtility.ApplyContrast(texture, cst.contrastval);

        if (cst.Saturate)
            SaturateUtility.ApplySaturation(texture, cst.saturateval);

        if (cst.InputLevel)
            InputLevelsUtility.ApplyInputLevels(texture, cst.whitePoint, cst.blackPoint, cst.gamma);

        if (cst.OutputLevel)
            OutputLevelsUtility.ApplyOutputLevels(texture, cst.minOutput, cst.maxOutput);
    }
}
