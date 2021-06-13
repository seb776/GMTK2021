using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Transform objectHolder;
    public bool die = false;

    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<AntAnimation>();
        animation.Run();
    }

    // Update is called once per frame
    void Update()
    {
        if(die)
        {
            Destroy(gameObject);
        }
    }

    public void TakeObject(GameObject go)
    {
        go.transform.parent = objectHolder;
        go.transform.localPosition = Vector3.zero;
    }

    public void DropObject(GameObject go)
    {
        var holder = objectHolder.gameObject.transform;
        if (holder.childCount > 0)
        {
            for (int i = 0; i < holder.childCount; i++)
            {
                var item = holder.GetChild(i);
                item.transform.parent = go.transform;

                var collider = item.transform.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                var fadeOut = item.gameObject.transform.GetChild(0).GetComponent<FadeOut>();
                if (fadeOut != null)
                {
                    fadeOut.Fade();
                }
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.tag);
    }
}
