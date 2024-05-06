using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public PlayerHealth player;

    void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerHealth>();
        }

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (player != null)
            {
                player.UpdateHealthUI();
            }
            else
            {
                Debug.LogError("Referência 'player' não definida.");
            }
        }
    }
}
