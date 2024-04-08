using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMovementAndHealth : MonoBehaviour
{
    [Header("Configurações de movimentação")]
    public float speed = 3f;
    public float followDistance = 5f;
    public float attackDistance = 2f;

    [Header("Configurações de vida")]
    public int maxHealth = 100;

    [Header("Configurações de knockback")]
    public float knockbackDistance = 0.5f;
    public float knockbackDuration = 0.2f;

    [Header("UI de Vida")]
    public static bool useHealthSlider = true;
    public Slider healthSlider;

    private Transform player;
    private int currentHealth;
    private bool isKnockedBack = false;
    private Animator animator;
    private DamageFeedback damageFeedback;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        damageFeedback = GetComponent<DamageFeedback>();
        animator = GetComponent<Animator>();

        if (useHealthSlider)
        {
            if (healthSlider != null)
            {
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }
        }
    }

    private void Update()
    {
        if (useHealthSlider == false)
        {
            healthSlider.gameObject.SetActive(false);
        }
        else
        {
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = currentHealth;
        }

        if (player == null || isKnockedBack)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= followDistance)
        {
            MoveTowardsPlayer();

        }
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
        else
        {
            transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
        }
    }


    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (damageFeedback != null)
        {
            damageFeedback.TakeDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ApplyKnockback());
        }
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
            transform.position = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / knockbackDuration));
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
        EnemyMovementAndHealth.useHealthSlider = enable;
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
