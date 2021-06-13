using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedShowAndHide : MonoBehaviour
{
    public float AnimationTime;
  
    private float elapsedTime = 0f;
    private Vector3 start;
    private Vector3 goal;
    private bool isAnimated = false;
    private bool isShowing = false;
    private bool isHiding = false;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAnimated)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, goal, elapsedTime / AnimationTime);
            if(AnimationTime < elapsedTime)
            {
                isAnimated = false;
                isShowing = false;
                isHiding = false;
            }
        }
    }

    public void Show()
    {
        if (!isShowing)
        {
            elapsedTime = 0f;
            start = gameObject.transform.localScale;
            goal = baseScale;
            isAnimated = true;
            isShowing = true;
        }
    }

    public void Hide()
    {
        if (!isHiding)
        {
            elapsedTime = 0f;
            start = gameObject.transform.localScale;
            goal = Vector3.zero;
            isAnimated = true;
            isHiding = true;
        }
    }
}
