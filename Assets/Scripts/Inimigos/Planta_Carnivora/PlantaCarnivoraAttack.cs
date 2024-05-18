using System.Collections;
using UnityEngine;

public class PlantaCarnivoraAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float distanciaAtaque = 10f;
    public float ataqueDelay = 1.0f;

    [Header("Components")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private float nextAttackTime = 0f;
    private Transform player;
    private Animator animator;
    private bool isDead = false;
    private bool canAttack = false;
    private bool isVisible = false; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isVisible && !isDead && Time.time >= nextAttackTime && IsPlayerInRange() && canAttack)
        {
            animator.SetTrigger("Attack");
            nextAttackTime = Time.time + ataqueDelay;
        }
    }

    public void ShootProjectile()
    {
        if (projectileSpawnPoint == null || isDead || !canAttack) return;

        Vector2 targetDirection = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * 10;
    }

    bool IsPlayerInRange()
    {
        return (transform.position - player.position).sqrMagnitude <= distanciaAtaque * distanciaAtaque;
    }

    IEnumerator EnableAttackWithDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canAttack = true;
        nextAttackTime = Time.time + ataqueDelay;
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
