using UnityEngine;

public class DetectAntHome : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
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
