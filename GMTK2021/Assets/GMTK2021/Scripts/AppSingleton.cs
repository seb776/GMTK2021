using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSingleton : Singleton<AppSingleton>
{
    public AntFarm SpawnerAllies;
    public float TimerSpawn;
    public bool spawnUnavailable;

    public float TimeRemain { get; set; }
    public float Score { get; set; }
    public GameObject Scout { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ResetGameParameters();
    }

    public void ResetGameParameters()
    {
        spawnUnavailable = false;
        TimerSpawn = 0f;
        Score = 0f;
        TimeRemain = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameEnded()
    {
        Debug.Log("Game finished ! :D");
    }

    public bool ScoutAlive()
    {
        return Scout != null;
    }
}
