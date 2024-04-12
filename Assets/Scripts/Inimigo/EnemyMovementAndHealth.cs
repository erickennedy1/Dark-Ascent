using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyMovementAndHealth : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float speed = 3f;
    public float followDistance = 5f;

    [Header("Configurações de vida")]
    public int maxHealth = 100;

    [Header("Configurações de knockback")]
    public float knockbackDistance = 0.5f;
    public float knockbackDuration = 0.2f;

    [Header("Elementos UI da vida")]
    public static bool useHealthSlider = true;
    public Slider healthSlider;

    private Transform player;
    private int currentHealth;
    private bool isKnockedBack = false;

    // Referencias de outros scripts
    private Animator animator;
    private DamageFeedback damageFeedback;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        damageFeedback = GetComponent<DamageFeedback>();
        InitializeHealthSlider();
    }

    void Update()
    {
        UpdateHealthSliderVisibility();
        if (!PlayerIsAvailable() || isKnockedBack) return;
        ProcessPlayerInteraction();
    }

    private void InitializeHealthSlider()
    {
        if (useHealthSlider && healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    private void UpdateHealthSliderVisibility()
    {
        if (healthSlider != null)
        {
            healthSlider.gameObject.SetActive(useHealthSlider);
            if (useHealthSlider)
            {
                healthSlider.value = currentHealth;
            }
        }
    }

    private bool PlayerIsAvailable()
    {
        return player != null;
    }

    private void ProcessPlayerInteraction()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= followDistance)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        AdjustFacingDirection();
    }

    private void AdjustFacingDirection()
    {
        transform.localScale = new Vector3((player.position.x > transform.position.x ? 1 : -1) * 0.8f, 0.8f, 0.8f);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (damageFeedback != null)
            damageFeedback.TakeDamage();

        if (currentHealth <= 0)
            Die();
        else
            StartCoroutine(ApplyKnockback());
    }

    private IEnumerator ApplyKnockback()
    {
        isKnockedBack = true;
        Vector3 knockbackDirection = (transform.position - player.position).normalized * knockbackDistance;
        Vector3 targetPosition = transform.position + knockbackDirection;
        float elapsedTime = 0;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isKnockedBack = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void ToggleEnemyHealthSliders(bool enable)
    {
        useHealthSlider = enable;
        EnemyMovementAndHealth[] allEnemies = FindObjectsOfType<EnemyMovementAndHealth>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.healthSlider != null)
            {
                enemy.healthSlider.gameObject.SetActive(enable);
            }
        }
    }
}
