using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public List<GameObject> Food;
    public float SpawnChance = 0.0004f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0f, 1f) < SpawnChance)
        {
            Vector2 position = Random.insideUnitCircle * 2;
            Instantiate(Food[Random.Range(0, Food.Count)], new Vector3(transform.position.x - position.x , 0.1f, transform.position.z - position.y), Quaternion.identity);
        }
    }
}
