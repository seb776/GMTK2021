using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonCouldown : MonoBehaviour
{
    public GameObject BackgroundImage;
    public float GrowthTime;
    public Vector3 MaxGrowth;

    public float CouldownTime;
    public List<TMP_Text> Couldown;

    private bool growth = false;
    private Vector3 goal;
    private Vector3 start;
    private float timeGrowing;

    private bool waitCouldown = false;
    private float timeCouldown;


    // Start is called before the first frame update
    void Start()
    {
        ChangeText(CouldownTime.ToString());
    }

    private void ChangeText(string t)
    {
        foreach (TMP_Text txt in Couldown)
        {
            txt.text = t;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (waitCouldown)
        {
            timeCouldown += Time.deltaTime;
            ChangeText((CouldownTime - timeCouldown).ToString("0"));
            if(timeCouldown > CouldownTime)
            {
                waitCouldown = false;
                ChangeText(CouldownTime.ToString());
            }
        } 
        if (growth)
        {
            Debug.Log(goal);
            timeGrowing += Time.deltaTime;
            BackgroundImage.transform.localScale = Vector3.Lerp(start, goal, timeGrowing / GrowthTime);

            if (timeGrowing > GrowthTime)
            {
                growth = false;
            }
        } 

    }

    public void MouseEnter()
    {
        Debug.Log("Enter");
        timeGrowing = 0f;
        start = BackgroundImage.transform.localScale;
        goal = MaxGrowth;
        growth = true;
    }

    public void MouseExit()
    {
        Debug.Log("Exit");
        timeGrowing = 0f;
        start = BackgroundImage.transform.localScale;
        goal = Vector3.one;
        growth = true;
    }

    public void MouseClick()
    {
        if (!waitCouldown)
        {
            timeCouldown = 0;
            waitCouldown = true;
            Debug.Log("Click");
        }
    }
}
