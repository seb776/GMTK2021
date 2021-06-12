using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPheromone : MonoBehaviour
{
    public GameObject Pheromone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Vector3 mouse = Input.mousePosition;
            mouse.y = Camera.main.transform.position.y;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mouse);
            Instantiate(Pheromone, new Vector3(objectPos.x, Pheromone.transform.localScale.y / 2, objectPos.z), Quaternion.identity);
        }
    }
}
