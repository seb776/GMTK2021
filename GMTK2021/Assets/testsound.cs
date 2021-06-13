using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testsound : MonoBehaviour
{
    public bool ABtn;
    public bool BBtn;
    public bool CBtn;
    void Update()
    {
        if (ABtn)
        {
            SoundManager.Instance.PlayDrop();
            ABtn = false;
        }
        if (BBtn)
        {
            SoundManager.Instance.PlayTake();
            BBtn = false;
        }
        if (CBtn)
        {
            SoundManager.Instance.PlayLarve();
            CBtn = false;
        }

    }
}
