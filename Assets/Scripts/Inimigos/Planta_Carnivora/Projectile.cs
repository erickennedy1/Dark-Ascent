using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configurações de dano")]
    public int damage = 1;

    private float lifetime = 2f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Destroy(gameObject, lifetime); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            animator.SetTrigger("Colidiu");

            Destroy(gameObject);
        }
    }
}
