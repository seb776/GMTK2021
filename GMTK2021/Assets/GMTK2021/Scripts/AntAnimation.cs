using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimation : MonoBehaviour
{
    [Header("Idle values")]
    public float Speed;
    public float MinVerticalScale;

    public GameObject AntBody;

    public void Right()
    {
        var curScale = AntBody.transform.localScale;
        AntBody.transform.localScale = new Vector3(1.0f, curScale.y, curScale.z);
    }

    public void Left()
    {
        var curScale = AntBody.transform.localScale;
        AntBody.transform.localScale = new Vector3(-1.0f, curScale.y, curScale.z);
    }

    public bool LeftButton;
    public bool RightButton;

    // Start is called before the first frame update
    void Start()
    {

                
    }

    // Update is called once per frame
    void Update()
    {
        var curScale = this.transform.localScale;
        var coef = Mathf.Sin(Time.realtimeSinceStartup * Speed) * 0.5f + 0.5f;
        this.transform.localScale = new Vector3(Mathf.Lerp(1.0f, MinVerticalScale, coef), curScale.y, curScale.z);

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
    }
}
