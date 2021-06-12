using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAntAnimation
{
    IDLE,
    RUN,
}

public class AntAnimation : MonoBehaviour
{
    public EAntAnimation _antAnimation { get; private set; }

    public float MinVerticalScale;

    [Header("Idle values")]
    public float IdleVerticalStretchSpeed;

    [Header("Foot values")]
    public float RunVerticalStretchSpeed;
    public float FootSpeed;
    public List<GameObject> FootFrames;

    public GameObject AntBody;

    public void Right()
    {
        var curScale = AntBody.transform.localScale;
        AntBody.transform.localScale = new Vector3(Mathf.Abs(curScale.x), curScale.y, curScale.z);
    }

    public void Left()
    {
        var curScale = AntBody.transform.localScale;
        AntBody.transform.localScale = new Vector3(-Mathf.Abs(curScale.x), curScale.y, curScale.z);
    }

    public bool LeftButton;
    public bool RightButton;
    public bool RunButton;
    public bool IdleButton;


    public void Run()
    {
        _antAnimation = EAntAnimation.RUN;
    }

    public void Idle()
    {
        _antAnimation = EAntAnimation.IDLE;
        _hideAllFootFrames();
        FootFrames[0].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Idle();
                
    }

    private void _hideAllFootFrames()
    {
        foreach (var footFrame in FootFrames)
        {
            footFrame.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var curScale = this.transform.localScale;

        float stretchSpeed = 1.0f;
        if (_antAnimation == EAntAnimation.IDLE)
            stretchSpeed = IdleVerticalStretchSpeed;
        else if (_antAnimation == EAntAnimation.RUN)
            stretchSpeed = RunVerticalStretchSpeed;

        var coef = Mathf.Sin(Time.realtimeSinceStartup * stretchSpeed) * 0.5f + 0.5f;
        this.transform.localScale = new Vector3(Mathf.Lerp(1.0f, MinVerticalScale, coef), curScale.y, curScale.z);

        if (_antAnimation == EAntAnimation.RUN)
        {
            _hideAllFootFrames();
            int footIndex = Mathf.FloorToInt(Time.realtimeSinceStartup * FootSpeed) % FootFrames.Count;
            FootFrames[footIndex].SetActive(true);
        }

        if (LeftButton)
        {
            LeftButton = false;
            Left();
        }

        if (RightButton)
        {
            RightButton = false;
            Right();
        }
        if (RunButton)
        {
            RunButton = false;
            Run();
        }
        if (IdleButton)
        {
            IdleButton = false;
            Idle();
        }
    }
}
