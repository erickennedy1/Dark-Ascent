using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configurações de vida")]
    public int maxHealth = 5;
    public float fadeSpeed = 0.5f;
    public bool seguirPlayer = true;

    private SpriteRenderer hpWarningSprite;
    private GameObject UI_VidaBaixa;
    private GameObject healthIconPrefab;
    private GameObject healthPanel;
    private Camera mainCamera;  
    private int currentHealth;

    void Awake()
    {
        healthPanel = GameObject.Find("LayoutHP");
        if (healthPanel.transform.childCount > 0)
        {
            healthIconPrefab = healthPanel.transform.GetChild(0).gameObject;
        }
        UI_VidaBaixa = GameObject.Find("UI_VidaBaixa");
        hpWarningSprite = UI_VidaBaixa.GetComponent<SpriteRenderer>();
        hpWarningSprite.color = new Color(hpWarningSprite.color.r, hpWarningSprite.color.g, hpWarningSprite.color.b, 0);
        mainCamera = Camera.main;  

        currentHealth = maxHealth;
        for (int i = 1; i < maxHealth; i++)
        {
            Instantiate(healthIconPrefab, healthPanel.transform);
        }
    }

    void LateUpdate()
    {
        if (seguirPlayer && mainCamera != null)
        {
            Vector3 cameraPosition = mainCamera.transform.position;
            UI_VidaBaixa.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, UI_VidaBaixa.transform.position.z);
        }

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
