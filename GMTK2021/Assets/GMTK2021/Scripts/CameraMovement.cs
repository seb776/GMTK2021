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
    public float MaxLeftRange = 6.8f;
    public float MinRightRange = -2.93f;

    public AnimatedShowAndHide LeftArrow;
    public AnimatedShowAndHide RightArrow;

    public float AnimationTime;

    private Vector3 start;
    private Vector3 target;
    private float elapsedTime;
    private bool isAnimated = false;
    private bool isMinized = false;
    private GameObject toScale;

    // Update is called once per frame
    void Update()
    {
        Vector2 resolution = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        var normalizedMousePos = Input.mousePosition.xy() / resolution;
        var centeredMousePos = (normalizedMousePos - new Vector2(0.5f, 0.5f)) * 2.0f;


        var deltaMousePos = Mathf.Abs(centeredMousePos.x) - BorderScrollThreshold;

        bool notOnMax = Camera.main.transform.position.z < MaxLeftRange;
        bool notOnMin = Camera.main.transform.position.z > MinRightRange;

        if (deltaMousePos > 0.0f && Mathf.Abs(centeredMousePos.x) <= 1.15f && (
                (notOnMax && centeredMousePos.x >= 0) || 
                (notOnMin && centeredMousePos.x < 0)
            )
        )
        {
            Camera.main.transform.position += Vector3.forward * Mathf.Pow(centeredMousePos.x, ExponentialSpeedPower) * Speed * Time.deltaTime;
        }

        if (!notOnMax) LeftArrow.Hide();
        else LeftArrow.Show();

        if (!notOnMin) RightArrow.Hide();
        else RightArrow.Show();
    }
}
