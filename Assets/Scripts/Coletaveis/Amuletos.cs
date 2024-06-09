using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amuletos : MonoBehaviour
{
    public Sprite vidaSprite;
    public Sprite manaSprite;
    public Sprite danoSprite;

    public SpriteRenderer spriteRenderer;
    public Collider2D collider2D;

    private int aleatorio; 

    private void Start()
    {
        aleatorio = Random.Range(0, 3);  
        AplicarAmuleto(aleatorio);
        StartCoroutine(ActivateColliderAfterDelay(1.5f));
    }

    private IEnumerator ActivateColliderAfterDelay(float delay)
    {
        collider2D.enabled = false;
        yield return new WaitForSeconds(delay);
        collider2D.enabled = true;
    }

    private void AplicarAmuleto(int aleatorio)
    {
        switch (aleatorio)
        {
            case 0:
                TrocarSprite(danoSprite);
                break;
            case 1:
                TrocarSprite(vidaSprite);
                break;
            case 2:
                TrocarSprite(manaSprite);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound("Player_Collect");
            AplicarAmuleto(aleatorio);  

            switch (aleatorio) 
            {
                case 0:
                    UpgradeDano(collision.gameObject);
                    break;
                case 1:
                    UpgradeVida(collision.gameObject);
                    break;
                case 2:
                    UpgradeMana(collision.gameObject);
                    break;
            }

            Destroy(gameObject);
        }
    }

    void UpgradeDano(GameObject player)
    {
        PlayerAttack playerAttack = player.GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.danoAtaque += 1;
        }
    }

    void UpgradeVida(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            bool maxHealthIncreased = false;

            if (playerHealth.maxHealth < 6)
            {
                playerHealth.maxHealth++;
                maxHealthIncreased = true;
            }

            if (playerHealth.currentHealth < playerHealth.maxHealth)
            {
                playerHealth.currentHealth = playerHealth.maxHealth;
                maxHealthIncreased = true;
            }

            if (maxHealthIncreased)
            {
                playerHealth.ResetHealthIcons();
                playerHealth.InitializeHealthIcons();
            }
            else
            {
                StartCoroutine(Aviso());
            }
        }
    }

    IEnumerator Aviso()
    {
        UI_Aviso.Instance.SetAviso(true, UI_Aviso.Instance.text[1]);
        yield return new WaitForSeconds(2f);
        UI_Aviso.Instance.SetAviso(false, string.Empty);
    }

    void UpgradeMana(GameObject player)
    {
        PlayerMana playerMana = player.GetComponent<PlayerMana>();
        if (playerMana != null)
        {
            playerMana.maxMana += 50;
            playerMana.currentMana = playerMana.maxMana;
            playerMana.UpdateManaPowerUPState();
        }
    }

    void TrocarSprite(Sprite novoSprite)
    {
        spriteRenderer.sprite = novoSprite;
    }
}
