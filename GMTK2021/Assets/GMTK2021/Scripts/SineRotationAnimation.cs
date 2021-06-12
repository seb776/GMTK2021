using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineRotationAnimation : MonoBehaviour
{
    private static float SPEED = 2.0f;
    public float Angle;
    public float AngleOffset;
    public float TimeOffset;
    public Vector3 AxisRotation;


    void Update()
    {
        float f = Mathf.Sin(SPEED * Time.realtimeSinceStartup+ TimeOffset) * 0.5f + 0.5f;
        this.transform.localRotation = Quaternion.AngleAxis(AngleOffset+Mathf.LerpAngle(-Angle, Angle, f), AxisRotation.normalized);
    }
}
