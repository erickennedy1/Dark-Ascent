using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private ParticulasAtaque particulasAtaque;

    [Header("Configurações de ataque")]
    public int danoAtaque = 10;
    public float distanciaAtaque = 2f;
    public bool canAttack = true;

    private bool podeAtacar = true;
    private Coroutine ataqueCouldown;

    [HideInInspector] public bool acabouDeAtacar = false;
    [HideInInspector] public float ultimoAtaqueHorizontal = 0f;
    [HideInInspector] public float ultimoAtaqueVertical = -1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        particulasAtaque = GetComponent<ParticulasAtaque>();  
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && podeAtacar && canAttack)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = transform.position;
            Vector2 direction = (mousePosition - playerPosition).normalized;

            SetAttackAnimationParameters(direction);

            animator.SetTrigger("Attack");
            acabouDeAtacar = true;

            ataqueCouldown = StartCoroutine(AttackCooldown());

            PerformAttack(direction);
        }
    }

    private void PerformAttack(Vector2 direction)
    {
        Vector2 attackPoint = (Vector2)transform.position + direction.normalized * distanciaAtaque / 2;
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint, distanciaAtaque, LayerMask.GetMask("Enemy", "Projectile", "Boss"));  // Inclui "Boss" na máscara de camada

        foreach (var hit in hitObjects)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyMovementAndHealth enemy = hit.GetComponent<EnemyMovementAndHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(danoAtaque);
                    particulasAtaque.SpawnParticles(enemy.transform.position);
                }
            }
            else if (hit.CompareTag("Projectile"))
            {
                Destroy(hit.gameObject);
            }
            else if (hit.CompareTag("Boss"))  
            {
                BossHealth boss = hit.GetComponent<BossHealth>();
                if (boss != null)
                {
                    boss.TakeDamage(danoAtaque);
                    particulasAtaque.SpawnParticles(hit.transform.position); 
                }
            }
        }
    }


    void SetAttackAnimationParameters(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetFloat("AttackHorizontal", Mathf.Sign(direction.x));
            animator.SetFloat("AttackVertical", 0);
            ultimoAtaqueHorizontal = Mathf.Sign(direction.x);
            ultimoAtaqueVertical = 0;
        }
        else
        {
            animator.SetFloat("AttackHorizontal", 0);
            animator.SetFloat("AttackVertical", Mathf.Sign(direction.y));
            ultimoAtaqueHorizontal = 0;
            ultimoAtaqueVertical = Mathf.Sign(direction.y);
        }
    }

    IEnumerator AttackCooldown()
    {
        podeAtacar = false;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        podeAtacar = true;
    }
    void OnDestroy()
    {
        if (ataqueCouldown != null)
        {
            StopCoroutine(ataqueCouldown);
        }
    }

    public void IncreaseDamage(int amount)
    {
        danoAtaque += amount;
    }
}