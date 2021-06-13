using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSingleton : Singleton<AppSingleton>
{
    public AntFarm SpawnerAllies;
    public float TimerSpawn;
    public bool spawnUnavailable;

    public Texture2D cursorTexture;
    public Texture2D cursorAttackTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update

    // Start is called before the first frame update
    void Start()
    {
        spawnUnavailable = false;
        TimerSpawn = 0f;
        ResetCursor();
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
}
