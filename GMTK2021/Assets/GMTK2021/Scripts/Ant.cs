using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AntFarm;

public class Ant : MonoBehaviour
{
    public Transform objectHolder;

    private bool isBusy = false;
    private bool isGroup = false;

    private int workerAntsNumber;
    private int warriorAntsNumber;

    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<AntAnimation>();
        animation.Run();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeObject(GameObject go)
    {
        // If the object doesn't have a parent and I'm not busy
        if (go.transform.parent == null)
        {
            if (!isBusy)
            {
                isBusy = true;

                go.transform.parent = objectHolder;
                go.transform.localPosition = Vector3.zero;

                var splineWalker = gameObject.GetComponent<SplineWalker>();
                if (splineWalker != null)
                {
                    splineWalker.Reverse();
                }
            }
        }
        else
        {
            if (isGroup || isBusy)
            {
                // Fusion ants
                var currentHolder = go.transform.parent.parent;
                if (currentHolder != null)
                {
                    var currentHolderAnt = go.transform.parent.parent.GetComponent<Ant>();
                    if (currentHolderAnt != null && currentHolderAnt.gameObject != gameObject)
                    {
                        currentHolderAnt.GroupAnts(gameObject);
                    }
                }
            }
        }
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

                isBusy = false;
            }
        }
    }

    public void GroupAnts(GameObject go)
    {
        /*
        if (go != null)
        {
            var otherAnt = go.GetComponent<Ant>();
            if (otherAnt != null && !otherAnt.isGroup)
            {
                isGroup = true;
                if (go.tag == EAntType.Worker.ToString())
                {
                    workerAntsNumber += 1;
                    Destroy(go.gameObject);
                }
                else if (go.tag == EAntType.Warrior.ToString())
                {
                    warriorAntsNumber += 1;
                    Destroy(go.gameObject);
                }

                Debug.Log($"Grouped! {workerAntsNumber} workers and {warriorAntsNumber} warriors");
            }
        }
        */
    }
}
