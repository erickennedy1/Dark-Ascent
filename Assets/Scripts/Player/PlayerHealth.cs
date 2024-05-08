using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de Vida")]
    public int maxHealth = 5;
    private int currentHealth;
    private GameObject healthPanel;

    void Awake()
    {
        healthPanel = GameObject.Find("Vida");
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
    }

    public void UpdateHealthUI()
    {
        for (int i = 0; i < healthPanel.transform.childCount; i++)
        {
            GameObject icon = healthPanel.transform.GetChild(i).gameObject;
            icon.SetActive(i < currentHealth);
        }
    }
}
