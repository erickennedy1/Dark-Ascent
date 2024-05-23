using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    private float lifetime = 1.3f;
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D enemyCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
        enemyCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            animator.SetTrigger("Colidiu");

            rb.Sleep();

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                enemyCollider.enabled = false;

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            animator.SetTrigger("Colidiu");
            rb.Sleep();
        
    }
    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
