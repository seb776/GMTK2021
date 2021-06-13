using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    public TMP_Text TextData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TextData = GetComponent<TMP_Text>();
        if (!TextData)
            return;
    }

    // Update is called once per frame
    void Update()
    {
        TextData.text = "Score: " + AppSingleton.Instance.Score;
        PlayerPrefs.SetFloat("Score", AppSingleton.Instance.Score);
    }
}
