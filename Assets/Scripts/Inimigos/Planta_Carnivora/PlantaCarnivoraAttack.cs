using UnityEngine;

public class PlantaCarnivoraAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float distanciaAtaque = 10f;
    [SerializeField] private float ataqueDelay = 1.0f;
    [SerializeField] private bool canAttack = false;

    [Header("Components")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private float nextAttackTime = 0f;
    private Transform player;
    private Animator animator;
    private bool isDead = false;

    private ISoundEnemy soundController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        soundController = GetComponent<ISoundEnemy>();
    }

    void Update()
    {
        if (!isDead && canAttack && IsPlayerInRange())
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + ataqueDelay;
            }
        }
    }

    public void ShootProjectile()
    {
        if (projectileSpawnPoint == null || isDead || !canAttack) return;

        soundController.PlayAttack();
        Vector2 targetDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * 10;
    }

    bool IsPlayerInRange()
    {
        return (transform.position - player.position).sqrMagnitude <= distanciaAtaque * distanciaAtaque;
    }

    public void SetAttack(bool state)
    {
        canAttack = state;
        nextAttackTime = Time.time;
    }
}
