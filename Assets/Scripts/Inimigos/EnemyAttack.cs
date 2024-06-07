using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configurações de ataque")]
    public int damageAmount = 1;
    private float attackCooldown = 3f;
    private float RangeAttackStart = 4f;
    private float AttackRange = 2.5f;
    private float dashSpeed = 15f;
    private bool isDead = false;
    [SerializeField] private bool canAttack = false;

    private Transform player;
    private Animator animator;
    private bool isReadyToAttack = true;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private ISoundEnemy soundController;
    private Vector2 dashTargetPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        soundController = GetComponent<ISoundEnemy>();

        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            player = obj.transform;
            playerHealth = obj.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (!isDead && canAttack)
        {
            if (isReadyToAttack && Vector2.Distance(transform.position, player.position) <= RangeAttackStart)
            {
                if (playerHealth.currentHealth > 0)
                {
                    StartCoroutine(PerformAttack());
                }
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        isReadyToAttack = false;
        animator.SetTrigger("Ataque");

        yield return new WaitForSeconds(attackCooldown);

        isReadyToAttack = true;
    }

    public void CauseDamage()
    {
        if (Vector2.Distance(transform.position, player.position) <= AttackRange)
        {
            if (playerHealth.currentHealth >= 1)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }
            }
        }
    }

    public void DashTowardsPlayer()
    {
        dashTargetPosition = player.position;
        soundController.PlayAttack();
        Vector2 direction = (dashTargetPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * dashSpeed;
        Invoke("StopDash", 0.2f);
    }

    private void StopDash()
    {
        rb.velocity = Vector2.zero;
    }

    public void EnemyDie()
    {
        isDead = true;
        Destroy(gameObject);
    }

    public void SetAttack(bool state)
    {
        canAttack = state;
    }

    void OnBecameVisible()
    {
        soundController.PlayIdle();
    }

    void OnBecameInvisible()
    {
        soundController.StopIdle();
    }
}
