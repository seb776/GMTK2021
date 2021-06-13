using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectProjectileHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var ant = other.gameObject.GetComponent<Ant>();
        if(ant != null)
        {
            Debug.Log("tango down");
        }
    }
}
