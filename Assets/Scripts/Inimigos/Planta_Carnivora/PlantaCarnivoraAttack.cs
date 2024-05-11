using System.Collections;
using UnityEngine;

public class PlantaCarnivoraAttack : MonoBehaviour
{
    [Header("Configurações de ataque")]
    public float shootingRange = 10f;
    private float attackDelay = 1.0f;  // Intervalo entre os ataques

    [Header("Componentes")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private LayerMask obstacleLayer;
    private float nextAttackTime = 0f;
    private Transform player;
    private Animator animator;
    private bool isDead = false;
    private float squaredShootingRange;

    void Start()
    {
        obstacleLayer = LayerMask.GetMask("Wall");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        squaredShootingRange = shootingRange * shootingRange;
    }

    void Update()
    {
        if (!isDead && player != null && InShootingRange() && !IsObstacleBetween())
        {
            if (Time.time >= nextAttackTime)
            {
                ShootProjectile();
                nextAttackTime = Time.time + attackDelay;
            }
        }
    }

    void ShootProjectile()
    {
        if (isDead || !projectileSpawnPoint) return;

        Vector2 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * 10; // Velocidade do projétil
    }

    bool InShootingRange()
    {
        return (transform.position - player.position).sqrMagnitude <= squaredShootingRange;
    }

    bool IsObstacleBetween()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstacleLayer);
        return hit.collider != null;
    }
}
