using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The custom settings class is used to store every custom data and
// operation applied to our texture.
[Serializable]
public class CustomSettings
{
    public CustomSettings()
    {
        GreyScale = false;
        CropSize = false;
        Blur = false;
        Negate = false;
        Brightness = false;
        Contrast = false;
        Saturate = false;
        InputLevel = false;
        OutputLevel = false;

        CropHighX = 0;
        CropHighY = 0;

        CropHeight = 0;
        CropWidth = 0;

        radius = 4;
        iterations = 2;

        brightval = 0;
        contrastval = 0;

        saturateval = 0f;

        blackPoint = 0;
        gamma = 0;
        whitePoint = 0;

        minOutput = 0;
        maxOutput = 0;
    }

    // Grayscale enabled

    public bool GreyScale;

    // Crop values

    public bool CropSize;
    public int CropHighX;
    public int CropHighY;
    public int CropHeight;
    public int CropWidth;

    // Blur values

    public bool Blur;
    public int radius;
    public int iterations;

    // Negate values
    public bool Negate;

    // Brightness values
    public bool Brightness;
    public int brightval;

    // Contrast values
    public bool Contrast;
    public double contrastval;

    // Saturation values
    public bool Saturate;
    public float saturateval;

    // Input Levels values
    public bool InputLevel;
    public float blackPoint;
    public float gamma;
    public float whitePoint;

    // Output Levels values
    public bool OutputLevel;
    public float minOutput;
    public float maxOutput;
}
