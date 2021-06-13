using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectAntInRadius : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var ant = other.GetComponent<Ant>();
        if (ant != null)
        {
            var path = GetComponent<SplineWalker>();
            if (path != null)
            {
                var spline = path.spline;
                if (spline != null && spline.Target == null)
                {
                    spline.ResetOrigin(transform);
                    spline.SetTarget(ant.gameObject);
                    path.active = true;
                }
            }
        }
    }
}
