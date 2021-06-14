using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public List<GameObject> Food;
    [Tooltip("Maximum time for spawning new food unit, in second")]
    public float MaxTimeSpawn = 20f;
    [Tooltip("Minimum time for spawning new food unit, in second")]
    public float MinTimeSpawn = 10f;
    [Tooltip("Radius of spawning zone")]
    public Vector2 radius;

    private float elapsedTime;
    private float spawnAt;
    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0f;
        spawnAt = 0f;
        if(MaxTimeSpawn < MinTimeSpawn)
        {
            Debug.LogError("FoodSpawner : Max Time Spawn < Min Time Spawn ! fix it by changing value !");
        }
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (spawnAt < elapsedTime)
        {
            Vector2 position = Random.insideUnitCircle * radius;
            Instantiate(Food[Random.Range(0, Food.Count)], new Vector3(transform.position.x - position.x , transform.position.y, transform.position.z - position.y), Quaternion.identity);
            spawnAt = Random.Range(MinTimeSpawn, MaxTimeSpawn);
            elapsedTime = 0f;
        }
    }
}
