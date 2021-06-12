using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;

    public float speed;

    public bool active;

    public bool lookForward;

    private float elapsedTime = 0;

    private float progress;

    public SplineWalkerMode mode;

    public float TotalLength { get; set; }

    private bool goingForward = true;

    private void Update()
    {
        if (active && spline != null)
        {
            elapsedTime += Time.deltaTime;
            Vector3 position;
            float splineLength = spline.TotalLength;
            if (goingForward)
            {
                progress = (speed * elapsedTime) / splineLength;
                position = spline.GetPoint(spline.FindPositionFromLookupTable(speed * elapsedTime));
                if (progress > 1f)
                {
                    elapsedTime = 0f;
                    progress = 1f;
                    switch (mode)
                    {
                        case SplineWalkerMode.Once:
                            active = false;
                            break;
                        case SplineWalkerMode.Loop:
                            progress = 0f;
                            break;
                        case SplineWalkerMode.PingPong:
                            goingForward = false;
                            var antAnimation = GetComponent<AntAnimation>();
                            if (antAnimation != null)
                            {
                                antAnimation.ToggleDirection();
                            }
                            else
                            {
                                var localScale = transform.localScale;
                                localScale.x *= -1;
                                transform.localScale = localScale;
                            }
                            break;
                    }
                }
            }
            else
            {
                progress = 1f - (speed * elapsedTime / splineLength);
                position = spline.GetPoint(spline.FindPositionFromLookupTable(splineLength - (speed * elapsedTime)));

                if (progress < 0f)
                {
                    progress = 0f;
                    elapsedTime = 0f;
                    goingForward = true;

                    var antAnimation = GetComponent<AntAnimation>();
                    if (antAnimation != null)
                    {
                        antAnimation.ToggleDirection();
                    }
                    else
                    {
                        var localScale = transform.localScale;
                        localScale.x *= -1;
                        transform.localScale = localScale;
                    }
                }
            }

            transform.localPosition = new Vector3(position.x, position.y, position.z);
            if (lookForward)
            {
                transform.LookAt(position + spline.GetDirection(progress));
            }
        }
    }
}