using UnityEngine;

public class DisableCollider : MonoBehaviour
{
    public void DisableCollider2D(){
        GetComponent<Collider2D>().enabled = false;
    }
}
