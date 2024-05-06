using UnityEngine;

public class HubTrigger : MonoBehaviour
{
    private Player player;


    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player.GetComponent<PlayerHealth>().SeguirPlayer(true);
        }
    }
}
