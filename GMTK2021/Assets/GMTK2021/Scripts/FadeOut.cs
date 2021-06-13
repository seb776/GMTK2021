using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float duration;

    private bool fade;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fade)
        {
            elapsedTime += Time.deltaTime;

            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                var objectColor = renderer.material.color;
                float fadeAmount = objectColor.a - (elapsedTime / duration);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                renderer.material.color = objectColor;
            }

            var particleSystem = GetComponentInChildren<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }

            if (elapsedTime >= duration)
            {
                if(gameObject.transform.parent != null)
                {
                    Destroy(gameObject.transform.parent.gameObject);
                }
                fade = false;
            }
        }
    }

    public void Fade()
    {
        fade = true;
    }
}
