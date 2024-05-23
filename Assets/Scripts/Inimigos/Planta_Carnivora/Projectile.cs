using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    private float lifetime = 1.3f;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    private bool hasCollided = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided) return;
        if (collision.tag == "Player")
        {
            hasCollided = true;
            collider2D.enabled = false;

            animator.SetTrigger("Colidiu");

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Optionally call DestroyProjectile() to stop and destroy the projectile immediately
            DestroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided) return;
        hasCollided = true;
        collider2D.enabled = false;

        animator.SetTrigger("Colidiu");

        // Optionally call DestroyProjectile() to stop and destroy the projectile immediately
        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        collider2D.enabled = false;
        Invoke("DelayedDestroy", 0.4f); // Delay the destroy by 0.4 seconds
    }

    private void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
