using UnityEngine;

public class AntFarm : MonoBehaviour
{
    public GameObject WarriorAntPrefab;
    public GameObject ScoutAntPrefab;
    public GameObject WorkerAntPrefab;
    public GameObject DefaultPathObject;

    public void Start()
    {
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

<<<<<<< HEAD
        GameObject gameObject1 = Instantiate(prefab);
        var splineCmp = gameObject1.GetComponent<SplineWalker>();
        splineCmp.spline = DefaultPathObject.GetComponent< BezierSpline>() as BezierSpline;
        if(EAntType.Scout == type)
        {
            AppSingleton.Instance.Scout = gameObject1;
        }
        return gameObject1.GetComponent<Ant>();
=======
        if(prefab != null)
        {
            GameObject gameObject1 = Instantiate(prefab);
            var splineCmp = gameObject1.GetComponent<SplineWalker>();
            gameObject1.tag = tag;
            splineCmp.spline = DefaultPathObject.GetComponent<BezierSpline>();
            return gameObject1.GetComponent<Ant>();
        }
        else
        {
            return null;
        }
>>>>>>> 294b5b2641a452bad458fab1ccf3204bfa33aee6
    }
}




