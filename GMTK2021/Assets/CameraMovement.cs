using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorSwizzle;

public class CameraMovement : MonoBehaviour
{
    public float BorderScrollThreshold;
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        Vector2 resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        var normalizedMousePos = Input.mousePosition.xy() / resolution;
        var centeredMousePos = (normalizedMousePos - new Vector2(0.5f, 0.5f)) * 2.0f;


        var deltaMousePos = Mathf.Abs(centeredMousePos.x) - BorderScrollThreshold;

        if (deltaMousePos > 0.0f)
        {
            Camera.main.transform.position += Vector3.forward * Mathf.Sign(centeredMousePos.x) * Speed * Time.deltaTime;
        }

    }
}
