using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Espinhos : MonoBehaviour
{
    public int dano = 1;
    private float tempo = 1.2f;
    private Collider2D colliderComponent; 

    void Start()
    {
        colliderComponent = GetComponent<Collider2D>(); 
        colliderComponent.enabled = false;
        Destroy(gameObject, tempo);
    }

    public void ActivateCollider()
    {
        colliderComponent.enabled = true;
    }

    public void DeactivateCollider()
    {
        colliderComponent.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(dano);
            }
        }
    }

}
