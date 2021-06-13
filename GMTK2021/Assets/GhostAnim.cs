using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnim : MonoBehaviour
{
    private static float MIN_X_DESTROY = -10.0f;

    void Start()
    {
        
    }



    void Update()
    {
        float speed = 1.0f;
        var pos = this.transform.position;
        float sideMove = Mathf.Sin(Time.realtimeSinceStartup * 2.0f + Mathf.Repeat((float)this.name.GetHashCode(), 5.0f)) * 0.05f;
        this.transform.position = new Vector3(pos.x - speed * Time.deltaTime, pos.y, pos.z + sideMove);
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, Mathf.Abs(this.transform.localScale.z) * Mathf.Sign(sideMove));
        if (this.transform.position.x < MIN_X_DESTROY)
        {
            Destroy(this.gameObject);
        }
    }
}
