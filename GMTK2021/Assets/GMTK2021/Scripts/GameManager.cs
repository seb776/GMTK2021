using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("Time of a game in seconds")]
    public float GameDuration;
    public List<TMP_Text> Timer;

    private float actualElapsed;
    // Start is called before the first frame update
    void Start()
    {
        actualElapsed = GameDuration;
        //timers = new List<TextMeshPro>();
        //foreach(GameObject obj in Timer)
        //{
        //    timers.Add(obj.GetComponent<TextMeshPro>());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        string actualTime = Mathf.FloorToInt(actualElapsed / 60).ToString("00") + ":" + Mathf.Abs(Mathf.FloorToInt(actualElapsed % 60)).ToString("00");
        foreach (TMP_Text txt in Timer)
        {
            txt.text = actualTime;
        }
        if(actualElapsed < 0f)
        {
            Debug.Log("Game finished");
        }
        actualElapsed -= Time.deltaTime;
    }

    public void ClickSpawn(int type)
    {
        Debug.Log("Spawn " + type);
    }
}
