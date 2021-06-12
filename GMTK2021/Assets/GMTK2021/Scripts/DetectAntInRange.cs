using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAntInRange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Upon collision with another GameObject, this GameObject will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        var ant = other.gameObject.GetComponent<Ant>();
        if(ant != null)
        {
            ant.TakeObject(this.gameObject);
        }
    }
}
