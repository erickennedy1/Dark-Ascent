using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbeColetavel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMana playerMana = collision.GetComponent<PlayerMana>();
            if (playerMana != null)
            {
                if (playerMana.currentMana < playerMana.maxMana)
                {
                    playerMana.RecuperarMana(10);
                }
            }

            Destroy(gameObject);
        }
    }
}
