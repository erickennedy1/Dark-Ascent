using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [Header("Configurações de ataque")]
    public int damageAmount = 2;
    public float attackCooldown = 2f;
    public float RangeAttackStart = 1.5f;
    public float AttackRange = 1f;
    public float dashSpeed = 10f;


    private Transform player;
    private Animator animator;
    private bool isReadyToAttack = true;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DrawAttackRange();  // Desenha o círculo de alcance de ataque

        if (isReadyToAttack && Vector2.Distance(transform.position, player.position) <= RangeAttackStart)
        {
            StartCoroutine(PerformAttack());
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
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
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

    public void OnAttackAnimationEnd()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    void DrawAttackRange()
    {
        float theta = 0;
        float x = AttackRange * Mathf.Cos(theta);
        float y = AttackRange * Mathf.Sin(theta);
        Vector3 pos = transform.position + new Vector3(x, y, 0);
        Vector3 newPos = pos;
        Vector3 lastPos = pos;

        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = AttackRange * Mathf.Cos(theta);
            y = AttackRange * Mathf.Sin(theta);
            newPos = transform.position + new Vector3(x, y, 0);
            Debug.DrawLine(lastPos, newPos, Color.red, 0.02f, false);
            lastPos = newPos;
        }

        // Conecta o último ponto ao primeiro
        Debug.DrawLine(lastPos, pos, Color.red, 0.02f, false);
    }
}
