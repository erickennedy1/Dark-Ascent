using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de vida")]
    public int maxHealth = 5;
    public float fadeSpeed = 0.5f; 

    private SpriteRenderer hpWarningSprite;
    private GameObject hpWarning;
    private GameObject healthIconPrefab;
    private GameObject healthPanel;
    private int currentHealth;

    void Start()
    {
        hpWarning = GameObject.Find("1HPWarning");
        hpWarningSprite = hpWarning.GetComponent<SpriteRenderer>();
        hpWarningSprite.color = new Color(hpWarningSprite.color.r, hpWarningSprite.color.g, hpWarningSprite.color.b, 0); 
    }

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

    void Update()
    {
        if (currentHealth <= 1 && hpWarningSprite.color.a < 1)
        {
            FadeIn();
        }
        else if (currentHealth > 1 && hpWarningSprite.color.a > 0)
        {
            FadeOut();
        }
    }

    private void FadeIn()
    {
        Color color = hpWarningSprite.color;
        color.a += fadeSpeed * Time.deltaTime; 
        hpWarningSprite.color = color;
    }

    private void FadeOut()
    {
        Color color = hpWarningSprite.color;
        color.a -= fadeSpeed * Time.deltaTime; 
        hpWarningSprite.color = color;
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
