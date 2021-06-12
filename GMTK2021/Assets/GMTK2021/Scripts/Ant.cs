using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Transform objectHolder;

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
        go.transform.parent = objectHolder;
        go.transform.localPosition = Vector3.zero;
    }

    public void DropObject()
    {
        if(objectHolder.transform.childCount > 0)
        {
            Destroy(objectHolder.transform.GetChild(0).gameObject);
        }
    }
}
