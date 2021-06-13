using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnButtons : MonoBehaviour
{

    public List<GameObject> panelScout;
    public List<GameObject> panelMain;
    public List<GameObject> panelTimer;
    public Vector3 speed;
    public float ShowCoordY = 68f;
    public float HideCoordY = -240f;
    public float ShowCoordYTimer = 98.5f;
    public float HideCoordYTimer = -240f;
    public float AccelerationTime = 5f;

    private float elapsedTimeHidden = 0f;
    private float elapsedTimeShow = 0f;

    public bool isWaiting { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(panelScout != null && panelMain != null)
        {
            if(AppSingleton.Instance.ScoutAlive())
            {
                MoveDown(panelScout, HideCoordY);
                MoveUp(panelMain, ShowCoordY);
            } else
            {
                MoveUp(panelScout, ShowCoordY);
                MoveDown(panelMain, HideCoordY);
            }
        }

        if(isWaiting)
        {
            MoveUp(panelTimer, ShowCoordYTimer);
        } else
        {
            MoveDown(panelTimer, HideCoordYTimer);
        }
    }

    private void MoveDown(List<GameObject> objects, float coordY)
    {
        elapsedTimeHidden += Time.deltaTime;
        if (elapsedTimeHidden > AccelerationTime) elapsedTimeHidden = AccelerationTime;
        foreach(GameObject ui in objects)
        {
            if (ui.transform.position.y > coordY)
            {
                ui.transform.position -= Vector3.Lerp(Vector3.zero, speed, elapsedTimeHidden / AccelerationTime) * Time.deltaTime;
            }
            if(ui.transform.position.y < coordY)
            {
                elapsedTimeHidden = 0f;
                ui.transform.position = new Vector3(ui.transform.position.x, coordY, ui.transform.position.z);
            }
        }
    }

    private void MoveUp(List<GameObject> objects, float coordY)
    {
        elapsedTimeShow += Time.deltaTime;
        if (elapsedTimeShow > AccelerationTime) elapsedTimeShow = AccelerationTime;
        foreach (GameObject ui in objects)
        {
            if (ui.transform.position.y < coordY)
            {
                ui.transform.position += Vector3.Lerp(Vector3.zero, speed, elapsedTimeShow / AccelerationTime) * Time.deltaTime;
            }
            if (ui.transform.position.y > coordY)
            {
                elapsedTimeShow = 0f;
                ui.transform.position = new Vector3(ui.transform.position.x, coordY, ui.transform.position.z);
            }
        }
    }
}
