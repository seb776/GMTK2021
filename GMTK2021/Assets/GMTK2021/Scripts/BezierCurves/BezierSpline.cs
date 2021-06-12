using System;
using System.Collections.Generic;
using UnityEngine;

public class BezierSpline : MonoBehaviour
{
    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private bool loop;

    List<float> accumulatedDistances = null;

    public float TotalLength
    {
        get
        {
            return GetSplineLength(1000, 1f);
        }
    }

    public bool Loop
    {
        get
        {
            return loop;
        }
        set
        {
            loop = value;
            if (value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }
        }
    }

    public int ControlPointCount
    {
        get
        {
            return points.Length;
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    public void SetControlPoint(int index, Vector3 point)
    {
        if (index % 3 == 0)
        {
            Vector3 delta = point - points[index];
            if (loop)
            {
                if (index == 0)
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = point;
                }
                else if (index == points.Length - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length)
                {
                    points[index + 1] += delta;
                }
            }
        }
        points[index] = point;
        EnforceMode(index);
    }

    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void Reset()
    {
        points = new Vector3[] {
            new Vector3(0f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(4f, 0f, 0f),
            new Vector3(6f, 0f, 0f)
        };

        modes = new BezierControlPointMode[] {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };

        accumulatedDistances = null;
    }

    public int CurveCount
    {
        get
        {
            return (points.Length - 1) / 3;
        }
    }

    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 2f;
        points[points.Length - 3] = point;
        point.x += 2f;
        points[points.Length - 2] = point;
        point.x += 2f;
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);

        if (loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }

        accumulatedDistances = null;
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }

    //Steps is the number of sections we are going to split the curve in
    //So the number of interpolated values are steps + 1
    //tEnd is where we want to stop measuring if we dont want to split the entire curve, so tEnd is maximum of 1
    public List<Vector3> SplitCurve(int steps, float tEnd)
    {
        //Store the interpolated values so we later can display them
        List<Vector3> interpolatedPositions = new List<Vector3>();

        //Loop between 0 and tStop in steps. If tStop is 1 we loop through the entire curve
        //1 step is minimum, so if steps is 5 then the line will be cut in 5 sections
        float stepSize = tEnd / (float)steps;

        float t = 0f;

        //+1 becuase wa also have to include the first point
        for (int i = 0; i < steps + 1; i++)
        {
            //Debug.Log(t);

            Vector3 interpolatedValue = GetPoint(t);

            interpolatedPositions.Add(interpolatedValue);

            t += stepSize;
        }

        return interpolatedPositions;
    }

    //Get the length of the curve with a naive method where we divide the
    //curve into straight lines and then measure the length of each line
    //tEnd is 1 if we want to get the length of the entire curve
    public float GetSplineLength(int steps, float tEnd)
    {
        //Split the ruve into positions with some steps resolution
        List<Vector3> CurvePoints = SplitCurve(steps, tEnd);

        //Calculate the length by measuring the length of each step
        float length = 0f;

        for (int i = 1; i < CurvePoints.Count; i++)
        {
            float thisStepLength = Vector3.Distance(CurvePoints[i - 1], CurvePoints[i]);

            length += thisStepLength;
        }

        return length;
    }

    //
    // Divide the curve into constant steps
    //
    //The problem with the t-value and Bezier curves is that the t-value is NOT percentage along the curve
    //So sometimes we need to divide the curve into equal steps, for example if we generate a mesh along the curve
    //So we have a distance along the curve and we want to find the t-value that generates that distance
    //Alternative 2
    //Create a lookup-table with distances along the curve, then interpolate these distances
    //This is faster but less accurate than using the iterative Newton–Raphsons method
    //But the difference from far away is barely noticeable
    //https://medium.com/@Acegikmo/the-ever-so-lovely-b%C3%A9zier-curve-eb27514da3bf
    public float FindPositionFromLookupTable(float d)
    {
        //Step 1. Find accumulated distances along the curve by using the bad t-value
        //This value can be pre-calculated
        if (accumulatedDistances == null)
        {
            accumulatedDistances = GetAccumulatedDistances(steps: 1000);
        }

        if (accumulatedDistances == null || accumulatedDistances.Count == 0)
        {
            throw new System.Exception("Cant interpolate to split bezier into equal steps");
        }

        //Step 2. Iterate through the table, to find at what index we reach the desired distance
        //It will most likely not end up exactly at an index, so we need to interpolate by using the 
        //index to the left and the index to the right

        //First we need special cases for end-points to avoid unnecessary calculations
        if (d <= accumulatedDistances[0])
        {
            return 0f;
        }
        else if (d >= accumulatedDistances[accumulatedDistances.Count - 1])
        {
            return 1f;
        }

        //Find the index to the left
        int indexLeft = 0;

        for (int i = 0; i < accumulatedDistances.Count - 1; i++)
        {
            if (accumulatedDistances[i + 1] > d)
            {
                indexLeft = i;

                break;
            }
        }

        //Step 3. Interpolate to get the t-value
        //Each distance also has a t-value we used to generate that distance
        //Each t in the list is increasing each step by: 
        float stepSize = 1f / (float)(accumulatedDistances.Count - 1);

        //With this step size we can calculate the t-value of the index
        float tValueL = indexLeft * stepSize;
        //The index right is just:
        float tValueR = (indexLeft + 1) * stepSize;

        //To interpolate we need a percentage between the left index and the right index in the distances array
        float percentage = (d - accumulatedDistances[indexLeft]) / (accumulatedDistances[indexLeft + 1] - accumulatedDistances[indexLeft]);

        float tGood = Lerp(tValueL, tValueR, percentage);

        return tGood;
    }

    float Lerp(float a, float b, float t)
    {
        t = Mathf.Clamp01(t);

        //Same as Mathf.Lerp(a, b, t);
        float interpolatedValue = (1f - t) * a + t * b;

        return interpolatedValue;
    }

    //
    // Calculate the accumulated total distances along the curve by walking along it with constant t-steps
    //
    public List<float> GetAccumulatedDistances(int steps)
    {
        //Step 1. Find positions on the curve by using the inaccurate t-value
        List<Vector3> positionsOnCurve = SplitCurve(steps, tEnd: 1f);

        //Step 2. Calculate the cumulative distances along the curve for each position along the curve 
        //we just calculated
        List<float> accumulatedDistances = new List<float>();

        float totalDistance = 0f;

        accumulatedDistances.Add(totalDistance);

        for (int i = 1; i < positionsOnCurve.Count; i++)
        {
            totalDistance += Vector3.Distance(positionsOnCurve[i], positionsOnCurve[i - 1]);

            accumulatedDistances.Add(totalDistance);
        }

        return accumulatedDistances;
    }
}
