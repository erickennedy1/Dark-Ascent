using UnityEngine;

public class AtaqueEspecial00 : MonoBehaviour
{
    private PlayerMana playerMana; // Referência ao script de mana do jogador

    void Start()
    {
        // Obtem o componente PlayerMana do GameObject
        playerMana = GetComponent<PlayerMana>();
        if (playerMana == null)
        {
            Debug.LogError("PlayerMana script not found on the GameObject!");
        }
    }

    void Update()
    {
        // Checa se a tecla Q foi pressionada
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Tenta usar 10 de mana
            playerMana.UsarMana(10);
        }
    }
}
