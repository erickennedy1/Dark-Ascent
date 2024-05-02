using System.Collections;
using UnityEngine;

public class PlantaCarnivoraAttack : MonoBehaviour
{
    [Header("Configurações de ataque")]
    private float shootingRange = 10f;
    private float attackCooldown = 5f;
    private float attackDelay = 0.15f;
    private float comboDelay = 5f;

    [Header("Componentes")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;

    private LayerMask obstacleLayer;
    private float nextAttackTime = 0f;
    private Transform player;
    private Animator animator;
    private System.Random random = new System.Random();
    private bool isDead = false; 

    void Start()
    {
        obstacleLayer = LayerMask.GetMask("Wall");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead && player != null && InShootingRange() && !IsObstacleBetween()) 
        {
            if (Time.time >= nextAttackTime)
            {
                StartCombo();
            }
        }
    }

    void StartCombo()
    {
        if (isDead) return; 

        int comboIndex = random.Next(0, 3);
        animator.SetBool("isComboActive", true);
        switch (comboIndex)
        {
            case 0:
                StartCoroutine(Combo1());
                break;
            case 1:
                StartCoroutine(Combo2());
                break;
            case 2:
                StartCoroutine(Combo3());
                break;
        }
        nextAttackTime = Time.time + comboDelay;
    }
    IEnumerator Combo1()
    {
        for (int y = 0; y < 2; y++)
        {
            yield return new WaitForSeconds(attackDelay);
            ShootProjectile();
            yield return new WaitForSeconds(0.2f);
        }
        EndCombo();
    }

    IEnumerator Combo2()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(attackDelay);
            ShootProjectile();
            yield return new WaitForSeconds(0.2f);
        }

        EndCombo();
    }

    IEnumerator Combo3()
    {
        for (int x = 0; x < 6; x++)
        {
            yield return new WaitForSeconds(attackDelay);
            ShootProjectile();
            yield return new WaitForSeconds(0.2f);
        }

        EndCombo();
    }

    void ShootProjectile()
    {
        if (isDead || !projectileSpawnPoint || IsObstacleBetween()) return;  

        Vector2 direction = (player.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * 10;
        }
    }

    void EndCombo()
    {
        if (isDead) return; 

        animator.SetBool("isComboActive", false);
        nextAttackTime = Time.time + comboDelay;
    }

    public void EnemyDie()
    {
        isDead = true;  
        animator.SetBool("isComboActive", false);
        Destroy(gameObject, 2.0f);
    }

    bool InShootingRange()
    {
        return Vector2.Distance(transform.position, player.position) <= shootingRange;
    }

    bool IsObstacleBetween()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstacleLayer);
        return hit.collider != null;
    }
}
