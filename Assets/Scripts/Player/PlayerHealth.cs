using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de vida")]
    public int maxHealth = 5;

    private GameObject healthIconPrefab;
    private GameObject healthPanel;
    private int currentHealth;

    void Awake()
    {
        healthPanel = GameObject.Find("LayoutHP");

        if (healthPanel.transform.childCount > 0)
        {
            healthIconPrefab = healthPanel.transform.GetChild(0).gameObject;
        }

        currentHealth = maxHealth;
        for (int i = 1; i < maxHealth; i++)
        {
            Instantiate(healthIconPrefab, healthPanel.transform);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        gameObject.GetComponent<DamageFeedback>().TakeDamage();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < healthPanel.transform.childCount; i++)
        {
            GameObject icon = healthPanel.transform.GetChild(i).gameObject;
            icon.SetActive(i < currentHealth);
        }
    }
}
