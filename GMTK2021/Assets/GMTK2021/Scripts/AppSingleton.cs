using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSingleton : Singleton<AppSingleton>
{
    public AntFarm SpawnerAllies;
    public float TimerSpawn;
    public bool spawnUnavailable;

<<<<<<< HEAD
    public float TimeRemain { get; set; }
    public float Score { get; set; }
    public GameObject Scout { get; set; }
=======
    public Texture2D cursorTexture;
    public Texture2D cursorAttackTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
>>>>>>> 294b5b2641a452bad458fab1ccf3204bfa33aee6

    // Start is called before the first frame update
    void Start()
    {
        ResetGameParameters();
    }

    public void ResetGameParameters()
    {
        spawnUnavailable = false;
        TimerSpawn = 0f;
<<<<<<< HEAD
        Score = 0f;
        TimeRemain = 0f;
=======
        ResetCursor();
>>>>>>> 294b5b2641a452bad458fab1ccf3204bfa33aee6
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

    }

    public void SetAttackCursor()
    {
        Cursor.SetCursor(cursorAttackTexture, hotSpot, cursorMode);

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
