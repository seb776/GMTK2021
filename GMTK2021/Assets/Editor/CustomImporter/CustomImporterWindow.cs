using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CustomImporterWindow : EditorWindow
{
    Texture2D myTex = null;
    CustomSettings loaded = null;
    Vector2 scrollpos = Vector2.zero;

    // Crop details
    int cropHighX = 0;
    int cropHighY = 0;
    int cropLowX = 0;
    int cropLowY = 0;

    // Blur details
    int radius = 4;
    int iterations = 2;

    // Brightness details
    int brightness = 1;

    // Constrast details
    double contrast = 0;

    // Saturate details
    float saturate = 0;

    // Levels details
    float blackPoint = 0;
    float whitePoint = 0;
    float gamma = 0;

    float minOutput = 0;
    float maxOutput = 0;

    [MenuItem("CustomImporter/Custom Texture Inspector")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomImporterWindow));
        
    }

    private void OnGUI()
    {
        GetTextureState();
        float margin = 5.0f;
        
        GUILayoutOption[] guiOptions = new GUILayoutOption[] { GUILayout.ExpandWidth(false) };
        scrollpos = GUILayout.BeginScrollView(scrollpos, guiOptions);
        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(margin);
        EditorGUILayout.LabelField("Custom Importer", EditorStyles.boldLabel, guiOptions);
        GUILayout.Space(margin * 2f);
        if (myTex != null)
        {
            // GrayScale Handling
            GUILayout.Label(myTex);
            GUILayout.Space(margin * 2f);
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Grayscale", EditorStyles.boldLabel, guiOptions);
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Apply Grayscale", guiOptions))
            { 
                HandleGrayScaleChange(true);
            }

            if (GUILayout.Button("Remove Grayscale", guiOptions))
            {
                    HandleGrayScaleChange(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Cropping Handling
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("Crop", EditorStyles.boldLabel, guiOptions);
            GUILayout.Space(margin);
            EditorGUILayout.LabelField("The texture dimensions are " + myTex.width + "x" + myTex.height, EditorStyles.boldLabel, guiOptions);

            GUILayout.Space(margin);
            EditorGUILayout.LabelField("Coordinate of top left point", EditorStyles.boldLabel, guiOptions);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X TopLeft :");
            cropHighX = EditorGUILayout.IntField(cropHighX, guiOptions);
            GUILayout.Label("Y TopLeft :");
            cropHighY = EditorGUILayout.IntField(cropHighY, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            EditorGUILayout.LabelField("Coordinate of bottom right point", EditorStyles.boldLabel, guiOptions);
            GUILayout.BeginHorizontal();
            GUILayout.Label("X BottomRight :");
            cropLowX = EditorGUILayout.IntField(cropLowX, guiOptions);
            GUILayout.Label("Y BottomLeft :");
            cropLowY = EditorGUILayout.IntField(cropLowY, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply Crop", guiOptions))
            {
                HandleCrop(true);
            }

            if (GUILayout.Button("Remove Crop", guiOptions))
            {
                HandleCrop(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);
            // Blur Handling
            EditorGUILayout.LabelField("Blur", EditorStyles.boldLabel, guiOptions);
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Blur radius", guiOptions);
            radius = EditorGUILayout.IntField(radius, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Blur iterations", guiOptions);
            iterations = EditorGUILayout.IntField(iterations, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply Blur", guiOptions))
            {
                HandleBlur(true);
            }

            if (GUILayout.Button("Remove Blur", guiOptions))
            {
                HandleBlur(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Negative Handling
            EditorGUILayout.LabelField("Negation", EditorStyles.boldLabel, guiOptions);
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            if (GUILayout.Button("Apply Negation", guiOptions))
            {
                HandleNegate(true);
            }

            if (GUILayout.Button("Remove Negation", guiOptions))
            {
                HandleNegate(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Brightness Handling
            EditorGUILayout.LabelField("Brightness", EditorStyles.boldLabel, guiOptions);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Brightness value", guiOptions);
            brightness = EditorGUILayout.IntField(brightness, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            if (GUILayout.Button("Apply Brightness", guiOptions))
            {
                HandleBrightness(true);
            }

            if (GUILayout.Button("Remove Brightness", guiOptions))
            {
                HandleBrightness(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Contrast Handling
            EditorGUILayout.LabelField("Contrast", EditorStyles.boldLabel, guiOptions);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Contrast value", guiOptions);
            contrast = EditorGUILayout.DoubleField(contrast, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            if (GUILayout.Button("Apply Contrast", guiOptions))
            {
                HandleContrast(true);
            }

            if (GUILayout.Button("Remove Contrast", guiOptions))
            {
                HandleContrast(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Saturation Handling
            EditorGUILayout.LabelField("Saturation", EditorStyles.boldLabel, guiOptions);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Saturation level", guiOptions);
            saturate = EditorGUILayout.FloatField(saturate, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(margin);
            if (GUILayout.Button("Apply Saturation", guiOptions))
            {
                HandleSaturation(true);
            }

            if (GUILayout.Button("Remove Saturation", guiOptions))
            {
                HandleSaturation(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Input levels handling
            EditorGUILayout.LabelField("Input Levels", EditorStyles.boldLabel, guiOptions);
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Black Point", guiOptions);
            blackPoint = EditorGUILayout.FloatField(blackPoint, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Gamma", guiOptions);
            gamma = EditorGUILayout.FloatField(gamma, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("White Point", guiOptions);
            whitePoint = EditorGUILayout.FloatField(whitePoint, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply Input Levels", guiOptions))
            {
                HandleInputLevels(true);
            }

            if (GUILayout.Button("Remove Input Levels", guiOptions))
            {
                HandleInputLevels(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
            GUILayout.Space(margin);

            // Output Levels Handling
            EditorGUILayout.LabelField("Output Levels", EditorStyles.boldLabel, guiOptions);
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Min Output", guiOptions);
            minOutput = EditorGUILayout.FloatField(minOutput, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Max Output", guiOptions);
            maxOutput = EditorGUILayout.FloatField(maxOutput, guiOptions);
            GUILayout.EndHorizontal();
            GUILayout.Space(margin);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply Output Levels", guiOptions))
            {
                HandleOutputLevels(true);
            }

            if (GUILayout.Button("Remove Output Levels", guiOptions))
            {
                HandleOutputLevels(false);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.Space(margin);
            GUILayout.Label("==================================");
        }
        else
            GUILayout.Label("No texture selected.");

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    /// <summary>
    /// We set or unset the output levels boolean.
    /// We then clamp min and max between 0 and 255
    /// </summary>
    /// <param name="changeOutputLevel"></param>
    private void HandleOutputLevels(bool changeOutputLevel)
    {
        loaded.OutputLevel = changeOutputLevel;
        minOutput = minOutput > 255 ? 255 : minOutput;
        minOutput = minOutput < 0 ? 0 : minOutput;
        maxOutput = maxOutput > 255 ? 255 : maxOutput;
        maxOutput = maxOutput < 0 ? 0 : maxOutput;

        loaded.minOutput = minOutput / 255;
        loaded.maxOutput = maxOutput / 255;

        UpdateLoaded();
    }

    /// <summary>
    /// We set or unset the input levels boolean.
    /// We then clamp min and max between 0 and 255
    /// </summary>
    /// <param name="changeInputLevel"></param>
    private void HandleInputLevels(bool changeInputLevel)
    {
        loaded.InputLevel = changeInputLevel;
        blackPoint = blackPoint > 255 ? 255 : blackPoint;
        blackPoint = blackPoint < 0 ? 0 : blackPoint;
        whitePoint = whitePoint > 255 ? 255 : whitePoint;
        whitePoint = whitePoint < 0 ? 0 : whitePoint;


        loaded.blackPoint = blackPoint / 255;
        loaded.whitePoint = whitePoint / 255;
        loaded.gamma = gamma;
        UpdateLoaded();
    }

    /// <summary>
    /// We set or unset the saturation activation, and then
    /// the saturate level in the custom settings object.
    /// </summary>
    /// <param name="changeSaturation"></param>
    private void HandleSaturation(bool changeSaturation)
    {
        loaded.Saturate = changeSaturation;
        loaded.saturateval = saturate;
        UpdateLoaded();
    }

    /// <summary>
    /// We set or unset the contrast value, update the proper value
    /// according to the formula in the custom settings, and clamp
    /// the contrast between -100 and 100
    /// </summary>
    /// <param name="changeContrast"></param>
    private void HandleContrast(bool changeContrast)
    {
        loaded.Contrast = changeContrast;

        if (contrast < -100)
            contrast = -100;
        if (contrast > 100)
            contrast = 100;

        loaded.contrastval = contrast;
        loaded.contrastval = (100 + loaded.contrastval) / 100;
        loaded.contrastval *= loaded.contrastval;

        UpdateLoaded();
    }

    /// <summary>
    /// We clamp the brightness between -255 and 255.
    /// Unset or set the brightness depending on the clicked
    /// button.
    /// </summary>
    /// <param name="changeBrightness"></param>
    private void HandleBrightness(bool changeBrightness)
    {
        loaded.Brightness = changeBrightness;

        if (brightness < -255)
            brightness = -255;

        if (brightness > 255)
            brightness = 255;

        loaded.brightval = brightness;
        UpdateLoaded();
    }

    /// <summary>
    /// We set or unset the negation
    /// </summary>
    /// <param name="changeNegate"></param>
    private void HandleNegate(bool changeNegate)
    {
        loaded.Negate = changeNegate;
        UpdateLoaded();
    }

    /// <summary>
    /// We set or unset the blur. We also change the radius
    /// and iterations value from the custom settings components.
    /// </summary>
    /// <param name="changeBlur"></param>
    private void HandleBlur(bool changeBlur)
    {
        loaded.Blur = changeBlur;
        loaded.radius = radius;
        loaded.iterations = iterations;
        UpdateLoaded();
    }
    
    /// <summary>
    /// Set or unset the crop. We then take the top left point
    /// the top right point, compute the desired height and width,
    /// and update the custom settings.
    /// </summary>
    /// <param name="changeCrop"></param>
    private void HandleCrop(bool changeCrop)
    {
        if (!changeCrop)
        {
            loaded.CropSize = changeCrop;
            UpdateLoaded();
            return;
        }
        int cropHeight = cropLowY - cropHighY;
        int cropWidth = cropLowX - cropHighX;

        if (cropHeight >= myTex.height || cropWidth >= myTex.width || cropHeight < 0 || cropWidth < 0)
            return;

        if (!checkValues(cropLowX, cropLowY, cropHighX, cropHighY))
            return;

        loaded.CropSize = changeCrop;
        loaded.CropHeight = cropHeight;
        loaded.CropWidth = cropWidth;
        loaded.CropHighX = cropHighX;
        loaded.CropHighY = cropHighY;

        UpdateLoaded();
    }

    /// <summary>
    /// Used in the cropping to avoid any problem.
    /// </summary>
    /// <param name="lowX"></param>
    /// <param name="lowY"></param>
    /// <param name="highX"></param>
    /// <param name="highY"></param>
    /// <returns></returns>
    private bool checkValues(int lowX, int lowY, int highX, int highY)
    {
        if (lowX < 0 || lowX > myTex.width)
            return false;

        if (lowY < 0 || lowY > myTex.height)
            return false;


        if (highX < 0 || highX > myTex.width)
            return false;

        if (highY < 0 || highY > myTex.height)
            return false;

        return true;
    }

    /// <summary>
    /// Set or unset grayscale.
    /// </summary>
    /// <param name="changeGrayScale"></param>
    private void HandleGrayScaleChange(bool changeGrayScale)
    {
        loaded.GreyScale = changeGrayScale;
        UpdateLoaded();
    }

    /// <summary>
    /// We get the file containing the custom settings that is loaded, depending
    /// on the texture. Then we convert the custom settings to a string,
    /// write it in a file. We then reimport the texture to trigger the changes and update
    /// the editor window.
    /// </summary>
    private void UpdateLoaded()
    {
        string finalPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID())).Replace("\\", "/") + "/" + Selection.activeObject.name + ".opimport";
        File.WriteAllText(finalPath, JsonUtility.ToJson(loaded));
        Debug.Log("Updated " + finalPath);
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID()));
        EditorWindow.GetWindow(typeof(CustomImporterWindow)).Repaint();
    }

    /// <summary>
    /// We check if a texture is selected.
    /// </summary>
    private void GetTextureState()
    {
        if (Selection.activeObject is Texture2D)
        {
            HandleCustomFile();
            myTex = AssetPreview.GetAssetPreview(Selection.activeObject);
        }

        else
        {
            myTex = null;
            loaded = null;
        }
    }

    /// <summary>
    /// We create the file containing an empty custom settings if non existant.
    /// If existant, we load it into the loaded attribute.
    /// </summary>
    private void HandleCustomFile()
    {
        string finalPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID())).Replace("\\", "/") + "/" + Selection.activeObject.name + ".opimport";

        if (!File.Exists(finalPath))
        {
            Debug.Log("Creating the file " + finalPath);

            CustomSettings cst = new CustomSettings();
            File.WriteAllText(finalPath, JsonUtility.ToJson(cst));
            loaded = cst;
        }

        else
        {
            Debug.Log("The file " + finalPath + "already exists. Loading it.");
            CustomSettings cst = JsonUtility.FromJson<CustomSettings>(File.ReadAllText(finalPath));
            loaded = cst;
        }
    }

    /// <summary>
    /// When an other object is selected, we repaint the window to update
    /// the texture view.
    /// </summary>
    private void OnSelectionChange()
    {
        EditorWindow.GetWindow(typeof(CustomImporterWindow)).Repaint();
    }

}
