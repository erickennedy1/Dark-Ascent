using UnityEngine;
using System.Collections;

public class EnemyMovementAndHealth : MonoBehaviour
{
    [Header("Configura��es de movimento")]
    public float speed = 3f;
    private float followDistance = 10f;

    [Header("Configura��es de vida")]
    public int maxHealth = 100;

    [Header("Configura��es de knockback")]
    private float knockbackDistance = 0.5f;
    private float knockbackDuration = 0.2f;

    private Transform player;
    private int currentHealth;
    private bool isKnockedBack = false;
    public bool knockbackBool = true;
    private bool morreu = false;
    public bool canMove = true;

    private Animator animator;
    private DamageFeedback damageFeedback;
    private PlantaCarnivoraAttack plantaAtaque;
    private EnemyAttack enemyAttack;
    private EnemyDrop enemyDrop;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        damageFeedback = GetComponent<DamageFeedback>();
        plantaAtaque = GetComponent<PlantaCarnivoraAttack>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyDrop = GetComponent<EnemyDrop>();
    }

    void Update()
    {
        if (!PlayerIsAvailable() || isKnockedBack) return;
        ProcessPlayerInteraction();
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
        if (!morreu && canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            AdjustFacingDirection();
        }
    }

    private void AdjustFacingDirection()
    {
        if (!morreu)
        {
            transform.localScale = new Vector3((player.position.x > transform.position.x ? 1 : -1) * 1.1f, 1.1f, 1.1f);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (damageFeedback != null)
            damageFeedback.TakeDamage();

        if (currentHealth <= 0)
            Die();
        else if (knockbackBool)
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
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isKnockedBack = false;
    }

    private void Die()
    {
        //enemyAttack.EnemyDie();
        animator.SetTrigger("Morrendo");
        morreu = true;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        enemyDrop.DropItem();
        Destroy(gameObject, 1f);
    }
}
