using UnityEngine;

public class SplineWalker : MonoBehaviour
{
    public BezierSpline spline;

    public float speed;

    public bool active;

    public float stopAt = 1f;

    private float target = 1f;

    public bool lookForward;

    private float elapsedTime = 0;

    private float progress;

    public SplineWalkerMode mode;

    public float TotalLength { get; set; }

    private bool goingForward = true;

    private void Start()
    {
        target = stopAt;
    }

    private void Update()
    {
        if (active && spline != null)
        {
            Vector3? position = null;
            //! Uncomment to follow target
            //spline.RefreshTarget();
            float splineLength = spline.TotalLength;
            if (goingForward)
            {
                elapsedTime += Time.deltaTime * speed;
                progress = Mathf.Clamp01(elapsedTime / splineLength);
                if (progress >= target)
                {
                    float actualProgress = progress;
                    progress = target;
                    switch (mode)
                    {
                        case SplineWalkerMode.Once:
                            elapsedTime = 0f;
                            active = false;

                            var spider = GetComponent<Spider>();
                            if(spider != null && spider.attackPath != null)
                            {
                                spider.attackPath.spline.ResetOrigin(spider.gameObject.transform);
                                spider.attackPath.active = true;
                            }
                            break;
                        case SplineWalkerMode.Loop:
                            elapsedTime = 0f;
                            progress = 0f;
                            break;
                        case SplineWalkerMode.PingPong:
                            Debug.Log(actualProgress);
                            if (actualProgress < 1f)
                                elapsedTime = (1f - actualProgress) * splineLength;
                            else
                                elapsedTime = 0f;
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
                else
                {
                    position = spline.GetPoint(spline.FindPositionFromLookupTable(progress *  splineLength));
                }
            }
            else
            {
                elapsedTime += Time.deltaTime * speed;
                progress = Mathf.Clamp01(elapsedTime / splineLength);
                if (progress >= 1f)
                {
                    Debug.Log(progress);
                    //progress = 0f;
                    elapsedTime = 0f;
                    target = stopAt;
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
                else
                {
                    position = spline.GetPoint(spline.FindPositionFromLookupTable(splineLength - (progress * splineLength)));
                }
            }

            if(position != null)
            {
                transform.localPosition = new Vector3(position.Value.x, position.Value.y, position.Value.z);
                if (lookForward)
                {
                    transform.LookAt(position.Value + spline.GetDirection(progress));
                }
            }
        }
    }

    public void Reverse()
    {
        target = progress;
    }
}