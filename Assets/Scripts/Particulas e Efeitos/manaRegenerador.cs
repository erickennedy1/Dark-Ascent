using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaRegenerador : MonoBehaviour
{
    public PlayerMana playerMana;

    void Start()
    {
        GameObject playerGameObject = GameObject.Find("Player");
        if (playerGameObject != null)
        {
            playerMana = playerGameObject.GetComponent<PlayerMana>();
            if (playerMana == null)
            {
                Debug.LogError("Componente PlayerMana não encontrado no GameObject!");
            }
            else
            {
                Debug.Log("Componente PlayerMana encontrado com sucesso.");
            }
        }
        else
        {
            Debug.LogError("GameObject com nome 'NomeDoGameObjectDoPlayer' não encontrado!");
        }
    }
    private void OnTriggerEnter2D(Collider2D item)
    {
        if (item.CompareTag("Player"))
        {
            if (playerMana != null)
            {
                playerMana.RecuperarMana(10);
            }
            else
            {
                Debug.LogError("Referência de PlayerMana não inicializada.");
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
