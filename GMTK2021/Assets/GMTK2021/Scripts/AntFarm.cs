using UnityEngine;

public class AntFarm : MonoBehaviour
{
    public GameObject WarriorAntPrefab;
    public GameObject ScoutAntPrefab;
    public GameObject WorkerAntPrefab;
    public GameObject DefaultPathObject;

    public void Start()
    {
        CreateAnt(EAntType.Warrior);
        CreateAnt(EAntType.Scout);
        CreateAnt(EAntType.Worker);
    }

    public enum EAntType
    {
        Scout = 0,
        Worker = 1,
        Warrior = 2,
    }

    public Ant CreateAnt(EAntType type)
    {
        GameObject prefab = null;
        switch (type)
        {
            case EAntType.Scout:
                prefab = ScoutAntPrefab;
                break;
            case EAntType.Warrior:
                prefab = WarriorAntPrefab;
                break;
            case EAntType.Worker:
                prefab = WorkerAntPrefab;
                break;
            default:
                return null;
        }

        GameObject gameObject1 = Instantiate(prefab);
        var splineCmp = gameObject1.GetComponent<SplineWalker>();
        splineCmp.spline = DefaultPathObject.GetComponent< BezierSpline>() as BezierSpline;
        return gameObject1.GetComponent<Ant>();
    }
}




