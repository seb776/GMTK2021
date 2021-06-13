using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AntFarm;

public class Ant : MonoBehaviour
{
    public Transform objectHolder;
    public bool die = false;

    private bool isBusy = false;

    private int workerAntsNumber = 0;
    private int warriorAntsNumber = 0;

    private int CurrentCapacity
    {
        get
        {
            return workerAntsNumber + warriorAntsNumber;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var animation = GetComponent<AntAnimation>();
        animation.Run();
    }

    // Update is called once per frame
    void Update()
    {
        if(die)
        {
            Destroy(gameObject);
        }
    }

    public void TakeObject(GameObject go)
    {
        // If the object doesn't have a parent
        if (go.transform.parent == null)
        {
            // I'm not busy, I can pick it up, otherwise I ignore it
            if (!isBusy)
            {
                isBusy = true;

                EAntType type;
                System.Enum.TryParse(gameObject.tag, out type);
                switch (type)
                {
                    case EAntType.Worker:
                        workerAntsNumber = 1;
                        warriorAntsNumber = 0;
                        break;

                    case EAntType.Warrior:
                        workerAntsNumber = 0;
                        warriorAntsNumber = 1;
                        break;
                }

                go.transform.parent = objectHolder;
                go.transform.localPosition = Vector3.zero;

                var particleSystem = go.GetComponentInChildren<ParticleSystem>();
                if(particleSystem != null)
                {
                    particleSystem.Play();
                }

                var splineWalker = gameObject.GetComponent<SplineWalker>();
                if (splineWalker != null)
                {
                    var consumable = go.GetComponent<Consumable>();
                    if (consumable != null)
                    {
                        splineWalker.speed = consumable.maxSpeed * ((float)CurrentCapacity / consumable.maxCapacity);
                        //Debug.Log(splineWalker.speed);
                    }
                    splineWalker.Reverse();
                }
            }
        }
        else if (go.transform.parent.parent != null)
        {
            //TODO: check capacity
            if (!isBusy)
            {
                // Fusion ants
                var currentHolderAnt = go.transform.parent.parent.GetComponent<Ant>();
                if (currentHolderAnt != null && currentHolderAnt.gameObject != gameObject)
                {
                    var consumable = go.GetComponent<Consumable>();
                    if (consumable != null && (currentHolderAnt.CurrentCapacity < consumable.maxCapacity))
                    {
                        currentHolderAnt.GroupAnts(gameObject);
                    }
                }
            }
        }
    }

    public void DropObject(GameObject go)
    {
        var holder = objectHolder.gameObject.transform;
        if (holder.childCount > 0)
        {
            for (int childIndex = 0; childIndex < holder.childCount; childIndex++)
            {
                var item = holder.GetChild(childIndex);
                item.transform.parent = go.transform;

                var fadeOut = item.gameObject.transform.GetChild(0).GetComponent<FadeOut>();
                if (fadeOut != null)
                {
                    fadeOut.Fade();
                }

                isBusy = false;
                // Spawn current number of ants
                for(int workerTotal = CurrentCapacity; workerTotal > 1; workerTotal--)
                {
                    if(Random.Range(0, 1) % 2 == 0)
                    {
                        if(workerAntsNumber > 1)
                        {
                            workerAntsNumber -= 1;
                            AppSingleton.Instance.SpawnerAllies.CreateAnt(EAntType.Worker);
                        }
                        else if(warriorAntsNumber > 0)
                        {
                            warriorAntsNumber -= 1;
                            AppSingleton.Instance.SpawnerAllies.CreateAnt(EAntType.Warrior);
                        }
                    }
                }
            }
        }
    }

    public void GroupAnts(GameObject go)
    {
        if (go != null)
        {
            var otherAnt = go.GetComponent<Ant>();
            if (otherAnt != null && !otherAnt.isBusy)
            {
                if (otherAnt.gameObject.tag == EAntType.Worker.ToString())
                {
                    workerAntsNumber += 1;
                    Destroy(go);
                }
                else if (otherAnt.gameObject.tag == EAntType.Warrior.ToString())
                {
                    warriorAntsNumber += 1;
                    Destroy(go);
                }

                var splineWalker = GetComponent<SplineWalker>();
                if (splineWalker != null)
                {
                    var consumable = objectHolder.gameObject.GetComponent<Consumable>();
                    if (consumable != null)
                    {
                        //splineWalker.speed = consumable.maxSpeed * ((float)CurrentCapacity / consumable.requiredCapacity);
                        //Debug.Log(splineWalker.speed);
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.tag);
    }
}
