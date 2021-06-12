using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPheromoneOffset : MonoBehaviour
{
    public MeshRenderer Renderer;

    void Update()
    {
        Renderer.material.SetFloat("_OffsetTime", this.transform.position.x + this.transform.position.z);        
    }
}
