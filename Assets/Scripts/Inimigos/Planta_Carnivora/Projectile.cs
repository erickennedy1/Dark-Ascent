using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;

    private float lifetime = 1.3f;
    private Animator animator;
    private Rigidbody2D rb;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Collider2D _trigger;
    private bool hasCollided = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasCollided) return;
        if (collision.tag == "Player")
        {
            hasCollided = true;
            animator.SetTrigger("Colidiu");

            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            DestroyProjectile();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided) return;
        hasCollided = true;

        animator.SetTrigger("Colidiu");

        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        _collider.enabled = false;
        _trigger.enabled = false;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        Invoke("DelayedDestroy", 0.4f); 
    }

    private void DelayedDestroy()
    {
        Destroy(gameObject);
    }
}
