using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineRotationAnimation : MonoBehaviour
{
    public float Speed = 2.0f;
    public float Angle;
    public float AngleOffset;
    public float TimeOffset;
    public Vector3 AxisRotation;

    public bool IsGrassAnim;

    public MeshRenderer GrassRenderer;

    private void Start()
    {
        //if (IsGrassAnim)
        //{
        //    const float _minInclusive = 0.9f;
        //    float r = Random.Range(_minInclusive, 1.0f);
        //    float g = Random.Range(_minInclusive, 1.0f);
        //    float b = Random.Range(_minInclusive, 1.0f);
        //    GrassRenderer.material.color = new Color(r, g, b, 1.0f);
        //}
        
    }


    void Update()
    {
        float f = Mathf.Sin(Speed * Time.realtimeSinceStartup+ TimeOffset) * 0.5f + 0.5f;
        if (IsGrassAnim)
        {
            f = ((Mathf.Sin(Speed * Time.realtimeSinceStartup + TimeOffset + this.transform.localPosition.x*0.5f + this.transform.localPosition.z) *0.25f
                +Mathf.Sin(Speed*Time.realtimeSinceStartup*0.5f+TimeOffset + this.transform.localPosition.x + this.transform.localPosition.z*0.5f) *0.5f
                + Mathf.Sin(Speed * Time.realtimeSinceStartup * 0.35f + TimeOffset+this.transform.localPosition.x + this.transform.localPosition.z)
                ) /3.0f)
                
                * 0.5f + 0.5f;
        }
        this.transform.localRotation = Quaternion.AngleAxis(AngleOffset+Mathf.LerpAngle(-Angle, Angle, f), AxisRotation.normalized);
    }
}
