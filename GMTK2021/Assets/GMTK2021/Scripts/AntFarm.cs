using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntFarm : MonoBehaviour
{
    public GameObject WarriorAntPrefab;
    public GameObject ScoutAntPrefab;
    public GameObject WorkerAntPrefab;
    public GameObject DefaultPathObject;
    public List<EAntType> ToRespawn;

    [Tooltip("This timer (in seconds) is only for respawning ant after come back with food")]
    public float TimeBetweenEachSpawn = 1f;

    private Coroutine respawningProcess;

    public void Start()
    {
        ToRespawn = new List<EAntType>();
        respawningProcess = null;
       /* CreateAnt(EAntType.Warrior);
        CreateAnt(EAntType.Scout);
        CreateAnt(EAntType.Worker);*/
    }

    public enum EAntType
    {
        Scout = 0,
        Worker = 1,
        Warrior = 2,
    }

    public void addToRespawn(int number, EAntType antType)
    {
        for(int i = 0; i < number; i++)
        {
            ToRespawn.Add(antType);
        }
        if(respawningProcess == null) respawningProcess = StartCoroutine(respawnGroups());
    }

    public IEnumerator respawnGroups()
    {
        while (ToRespawn.Count > 0)
        {
            CreateAnt(ToRespawn[0]);
            ToRespawn.RemoveAt(0);
            yield return new WaitForSeconds(TimeBetweenEachSpawn);
        }
        respawningProcess = null;
    }

    public Ant CreateAnt(EAntType type)
    {
        GameObject prefab = null;
        var tag = string.Empty;
        switch (type)
        {
            case EAntType.Scout:
                if (AppSingleton.Instance.ScoutAlive()) return null;
                prefab = ScoutAntPrefab;
                tag = EAntType.Scout.ToString();
                break;
            case EAntType.Warrior:
                prefab = WarriorAntPrefab;
                tag = EAntType.Warrior.ToString();
                break;
            case EAntType.Worker:
                prefab = WorkerAntPrefab;
                tag = EAntType.Worker.ToString();
                break;
            default:
                return null;
        }

        if(prefab != null)
        {
            GameObject gameObject1 = Instantiate(prefab);
            var splineCmp = gameObject1.GetComponent<SplineWalker>();
            gameObject1.tag = tag;
            splineCmp.spline = DefaultPathObject.GetComponent<BezierSpline>();
            if(EAntType.Scout == type)
            {
                AppSingleton.Instance.Scout = gameObject1;
            }
            return gameObject1.GetComponent<Ant>();
        }
        else
        {
            return null;
        }
    }
}




