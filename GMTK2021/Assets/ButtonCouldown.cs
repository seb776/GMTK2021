using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonCouldown : MonoBehaviour
{
    public GameObject BackgroundImage;
    public GameObject Larve;
    public float GrowthTime;
    public Vector3 MaxGrowth;

    public float CouldownTime;
    public List<TMP_Text> Couldown;

    private bool growth = false;
    private Vector3 goal;
    private Vector3 start;
    private float timeGrowing;

    private AppSingleton GameData;
    private bool isMasterTimer = false;

    private SpawnButtons buttonManager; 

    // Start is called before the first frame update
    void Start()
    {
        GameData = AppSingleton.Instance;
        ChangeText(CouldownTime.ToString());
        buttonManager = transform.parent.parent.GetComponent<SpawnButtons>();
    }

    private void ChangeText(string t)
    {
        foreach (TMP_Text txt in Couldown)
        {
            txt.text = t;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameData.spawnUnavailable)
        {
            if (isMasterTimer)
            {
                GameData.TimerSpawn += Time.deltaTime;
                ChangeText((CouldownTime - GameData.TimerSpawn).ToString("0"));
                if (AppSingleton.Instance.TimerSpawn > CouldownTime)
                {
                    GameData.spawnUnavailable = false;
                    isMasterTimer = false;
                    buttonManager.isWaiting = false;
                    SoundManager.Instance.PlayLarve();
                }
                var localScale = Larve.transform.localScale;
                localScale = Vector3.Lerp(Vector3.zero, Vector3.one, GameData.TimerSpawn / CouldownTime);
                Larve.transform.localScale = localScale;
            }
        }
        else
        {
            if (growth)
            {
                timeGrowing += Time.deltaTime;
                BackgroundImage.transform.localScale = Vector3.Lerp(start, goal, timeGrowing / GrowthTime);
                Larve.transform.localScale = Vector3.Lerp(start, goal, timeGrowing / GrowthTime);

                if (timeGrowing > GrowthTime)
                {
                    growth = false;
                }
            }
        }
    }

    public void MouseEnter()
    {
        timeGrowing = 0f;
        start = BackgroundImage.transform.localScale;
        goal = MaxGrowth;
        growth = true;
    }

    public void MouseExit()
    {
        timeGrowing = 0f;
        start = BackgroundImage.transform.localScale;
        goal = Vector3.one;
        growth = true;
    }

    public void MouseClickLaunch(int type)
    {
        if (!GameData.spawnUnavailable && GameData.SpawnerAllies.CreateAnt((AntFarm.EAntType)type))
        {
            GameData.TimerSpawn = 0;
            GameData.spawnUnavailable = true;
            isMasterTimer = true;
            buttonManager.isWaiting = true;
        }
    }
}
