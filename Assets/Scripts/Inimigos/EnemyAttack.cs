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
    public bool isDead = false;
    public bool canAttack = false;
    private bool isVisible = false;

    private Transform player;
    private Animator animator;
    private bool isReadyToAttack = true;
    private Rigidbody2D rb;
    private PlayerHealth playerHealth;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();

        GameObject obj = GameObject.Find("Player");
        if (obj != null)
        {
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
        Vector2 direction = (player.position - transform.position).normalized;
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
        //animator.SetTrigger("Die");
        Destroy(gameObject);
    }

    IEnumerator EnableAttackWithDelay()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

    void OnBecameVisible()
    {
        isVisible = true;
        StartCoroutine(EnableAttackWithDelay());
    }

    void OnBecameInvisible()
    {
        isVisible = false;
        canAttack = false;
    }
}
