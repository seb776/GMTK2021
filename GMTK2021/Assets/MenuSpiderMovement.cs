using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpiderMovement : MonoBehaviour
{
    public float Speed;
    public float AnimLength;
    public float Jumps;

    public float OriginalYPos;
    public float DownYPos;

    void Update()
    {
        float t = Time.realtimeSinceStartup * Speed;
        float f = 1.0f - (Mathf.Sin(t * Jumps) * (1.0f - Mathf.Clamp01(t * AnimLength)));
        float newY = Mathf.Lerp(OriginalYPos, DownYPos, f*0.5f);
        var pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, newY, pos.z);
    }
}
