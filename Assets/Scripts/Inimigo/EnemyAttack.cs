using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configurações de ataque")]
    public int damageAmount = 2;
    public float attackCooldown = 2f;
    public float attackRange = 1.5f;
    public Transform player;

    private float lastAttackTime;
    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        lastAttackTime = -attackCooldown;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isAttacking && Vector2.Distance(transform.position, player.position) <= attackRange && Time.time - lastAttackTime > attackCooldown)
        {
            isAttacking = true;
            animator.SetTrigger("Ataque");
        }
    }

    public void CauseDamage()
    {
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
                lastAttackTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isAttacking)
        {
            lastAttackTime = -attackCooldown;
        }
    }

    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
}
