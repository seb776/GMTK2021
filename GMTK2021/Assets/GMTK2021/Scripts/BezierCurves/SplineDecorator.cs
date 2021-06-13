using System.Collections.Generic;
using UnityEngine;

public class SplineDecorator : MonoBehaviour
{

    public BezierSpline spline;

    private int previousFrequency;

    public int frequency;

    public bool lookForward;

    public List<Transform> items;

    private List<Transform> elements = new List<Transform>();

    private void Awake()
    {
        previousFrequency = frequency;
        PopulateItems();
    }

    private void Update()
    {
        if (previousFrequency != frequency)
        {
            previousFrequency = frequency;
            foreach(var element in elements)
            {
                if(element != null && element.gameObject != null)
                {
                    Destroy(element.gameObject);
                }
            }
            PopulateItems();
        }
    }

    private void PopulateItems()
    {
        if (spline != null)
        {
            if (frequency <= 0 || items == null || items.Count == 0)
            {
                return;
            }

            float stepSize = frequency * items.Count;
            if (spline.Loop || stepSize == 1)
            {
                stepSize = 1f / stepSize;
            }
            else
            {
                stepSize = 1f / (stepSize - 1);
            }

            elements.Clear();
            for (int p = 0, f = 0; f < frequency; f++)
            {
                for (int i = 0; i < items.Count; i++, p++)
                {
                    Transform item = Instantiate(items[i]);
                    elements.Add(item);
                    Vector3 position = spline.GetPoint(spline.FindPositionFromLookupTable(p * stepSize * spline.TotalLength));
                    item.transform.localPosition = position;
                    if (lookForward)
                    {
                        item.transform.LookAt(position + spline.GetDirection(p * stepSize));
                    }
                    item.transform.parent = transform;
                }
            }
        }
    }
}
