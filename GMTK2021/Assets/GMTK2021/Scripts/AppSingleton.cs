using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSingleton : Singleton<AppSingleton>
{
    public AntFarm SpawnerAllies;
    public float TimerSpawn;
    public bool spawnUnavailable;
    // Start is called before the first frame update
    void Start()
    {
        spawnUnavailable = false;
        TimerSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
