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
    public Collider GroundCollider;

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

        var leftRay = Camera.main.ScreenPointToRay(Vector3.zero);
        var rightRay = Camera.main.ScreenPointToRay(Vector3.Scale(Vector3.one, new Vector3(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight, 1.0f)));

        float curLeftZ = Camera.main.transform.position.z;
        float curRightZ = Camera.main.transform.position.z;
        RaycastHit groundHit = new RaycastHit();
        if (GroundCollider.Raycast(leftRay, out groundHit, 100.0f))
            curLeftZ = groundHit.point.z;
        if (GroundCollider.Raycast(rightRay, out groundHit, 100.0f))
            curRightZ = groundHit.point.z;

        bool leftOut = curLeftZ < MaxLeftRange;
        bool rightOut = curRightZ > MinRightRange;

        float direction = Mathf.Sign(centeredMousePos.x);
        var offset = Vector3.forward * Mathf.Pow(Mathf.Abs(centeredMousePos.x), ExponentialSpeedPower) * Speed * Time.deltaTime;
        if (curLeftZ > MaxLeftRange && curRightZ < MinRightRange)
            Camera.main.transform.position += direction * offset;
        if (curLeftZ < MaxLeftRange && direction > 0.0f)
            Camera.main.transform.position += direction * offset;
        if (curRightZ > MinRightRange && direction < 0.0f)
            Camera.main.transform.position += direction * offset;

        if (rightOut)
        {
            LeftArrow.Hide();
        }
        else LeftArrow.Show();

        if (leftOut)
        {
            RightArrow.Hide();
        }
        else RightArrow.Show();
    }
}
