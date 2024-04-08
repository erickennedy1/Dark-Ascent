using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de vida")]
    public int maxHealth = 5;

    [Header("Componentes")]
    public GameObject healthIconPrefab;
    public GameObject healthPanel;

    private int currentHealth;

    void Awake()
    {
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
            if (i >= currentHealth)
            {
                Animator iconAnimator = icon.GetComponent<Animator>();
                if (iconAnimator != null)
                {

                    iconAnimator.SetTrigger("Destroy");

                    StartCoroutine(RemoveHealthIconAfterDelay(icon, 0.25f));
                }
                else
                {
                    icon.SetActive(false);
                }
            }
            else
            {
                icon.SetActive(true);
            }
        }
    }

    IEnumerator RemoveHealthIconAfterDelay(GameObject icon, float delay)
    {

        yield return new WaitForSeconds(delay);

        icon.SetActive(false);
    }
}
