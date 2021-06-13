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
    private AppSingleton GameData;
    // Start is called before the first frame update
    void Start()
    {
        GameData = AppSingleton.Instance;
        actualElapsed = GameDuration;
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
            GameData.GameEnded();
        }
        actualElapsed -= Time.deltaTime;
        GameData.TimeRemain = actualElapsed;
    }
}
