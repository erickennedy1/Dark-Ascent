using System.Collections;
using UnityEngine;

public class PlantaCarnivoraAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float distanciaAtaque = 10f;
    public float ataqueDelay = 1.0f;
    public float enableAttackDelay = 2.0f;

    [Header("Components")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private float nextAttackTime = 2f;
    private Transform player;
    private Animator animator;
    private bool isDead = false;
    public bool canAttack = true;
    private bool canAttack2 = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead && Time.time >= nextAttackTime && IsPlayerInRange() && canAttack2)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + ataqueDelay;
        }

        if (canAttack && !canAttack2)
        {
            StartCoroutine(EnableAttackWithDelay());
        }
    }

    public void ShootProjectile()
    {
        if (projectileSpawnPoint == null || isDead || !canAttack2) return; 

        Vector2 targetDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * 10;
    }

    bool IsPlayerInRange()
    {
        float squaredDistance = (transform.position - player.position).sqrMagnitude;
        return squaredDistance <= distanciaAtaque * distanciaAtaque;
    }

    IEnumerator EnableAttackWithDelay()
    {
        yield return new WaitForSeconds(enableAttackDelay);
        canAttack2 = true;
    }
}
