using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public float maxSpeed;

    [SerializeField]
    private int _maxCapacity;
    public int maxCapacity
    {
        get { return Mathf.Max(1, _maxCapacity); }
        set
        {
            _maxCapacity = value;
        }
    }

    public int ScorePoints;
    public int TimeBonus;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
