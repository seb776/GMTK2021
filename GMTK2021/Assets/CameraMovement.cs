using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorSwizzle;

public class CameraMovement : MonoBehaviour
{
    public float BorderScrollThreshold = 0.6f;
    public float Speed = 12;
    [Tooltip("Must be odd number !")]
    public float ExponentialSpeedPower = 7;
    public float IgnoreValueUpperThan = 1.1f;

    // Update is called once per frame
    void Update()
    {
        Vector2 resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        var normalizedMousePos = Input.mousePosition.xy() / resolution;
        var centeredMousePos = (normalizedMousePos - new Vector2(0.5f, 0.5f)) * 2.0f;


        var deltaMousePos = Mathf.Abs(centeredMousePos.x) - BorderScrollThreshold;

        if (deltaMousePos > 0.0f && Mathf.Abs(centeredMousePos.x) <= 1.15f)
        {
            Camera.main.transform.position += Vector3.forward * Mathf.Pow(centeredMousePos.x, ExponentialSpeedPower) * Speed * Time.deltaTime;
        }
    }
}
