using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppSingleton : Singleton<AppSingleton>
{
    public AntFarm SpawnerAllies;
    public float TimerSpawn;
    public bool spawnUnavailable;

    public float TimeRemain { get; set; }
    public float Score { get; set; }
    public GameObject Scout { get; set; }
    void Start()
    {
        ResetGameParameters();
    }

    public void ResetGameParameters()
    {
        spawnUnavailable = false;
        TimerSpawn = 0f;
        Score = 0f;
        PlayerPrefs.SetFloat("Score", 0);
        TimeRemain = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameEnded()
    {
        SceneManager.LoadScene("GMTK2021/Scenes/EndGame");
    }

    public bool ScoutAlive()
    {
        return Scout != null;
    }
}
