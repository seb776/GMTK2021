using UnityEngine;

public class DetectAntHome : MonoBehaviour
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
        var ant = other.gameObject.GetComponent<Ant>();
        if (ant != null)
        {
            ant.DropObject(gameObject);
        }
    }
}
