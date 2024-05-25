using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public GameObject itemPrefab;

    public void DropItem()
    {
        if (Random.value < 0.5f) 
        {
            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, transform.position, Quaternion.identity);
            }

        }
    }
}