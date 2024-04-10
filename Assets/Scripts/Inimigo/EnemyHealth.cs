using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Configurações de vida")]
    public int maxHealth = 100;

    [Header("Configurações de knockback")]
    public float knockbackDistance = 0.5f; 
    public float knockbackDuration = 0.2f; 

    private int currentHealth;
    public bool IsKnockedBack { get; private set; }
    private Transform player;
    private DamageFeedback damageFeedback;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        damageFeedback = GetComponent<DamageFeedback>();
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
        IsKnockedBack = true;

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

        IsKnockedBack = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
