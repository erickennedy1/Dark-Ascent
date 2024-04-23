using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    public void OnClose()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        GetComponent<Collider2D>().enabled = true;
    }

    public void OnOpen()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        GetComponent<Collider2D>().enabled = false;
    }
}
