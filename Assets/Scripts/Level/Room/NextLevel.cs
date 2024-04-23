using UnityEngine;

public class NextLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
            GameController.instance.NextLevel();            
    }
}