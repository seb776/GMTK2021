using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var TextData = GetComponent<TMP_Text>();
        if (!TextData)
            return;

        TextData.text = PlayerPrefs.GetFloat("Score").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
