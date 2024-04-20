using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private GameObject sparksAnimationObject;

    [Header("Configurações de ataque")]
    public int damageAmount = 10;
    private float attackRange = 2f;
    private float attackParticules = 0.5f;


    private bool canAttack = true;
    private Coroutine attackCooldownCoroutine;
    private AtaqueEspecial00 specialAttackScript;

    [HideInInspector] public bool justAttacked = false;
    [HideInInspector] public float lastAttackHorizontal = 0f;
    [HideInInspector] public float lastAttackVertical = -1f;

    void Start()
    {
        animator = GetComponent<Animator>();
        specialAttackScript = GetComponent<AtaqueEspecial00>();

        Transform sparksTransform = transform.Find("Efeito de ataque");

        sparksAnimationObject = sparksTransform.gameObject;

    }

    void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPosition = transform.position;
            Vector2 direction = (mousePosition - playerPosition).normalized;

            SetAttackAnimationParameters(direction);

            animator.SetTrigger("Attack");
            justAttacked = true;

            attackCooldownCoroutine = StartCoroutine(AttackCooldown());

            PerformAttack(direction);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, attackRange, LayerMask.GetMask("Enemy"));
            if (hit.collider != null)
            {
                Vector2 hitDirection = (hit.transform.position - transform.position).normalized;
                float angle = Vector2.Angle(direction, hitDirection);

                if (angle <= 45f)
                {
                    EnemyMovementAndHealth enemy = hit.collider.GetComponent<EnemyMovementAndHealth>();
                    if (enemy != null)
                    {
                        float distance = Vector2.Distance(transform.position, enemy.transform.position);
                        if (distance <= attackRange)
                        {
                            enemy.TakeDamage(damageAmount);
                            sparksAnimationObject.transform.position = enemy.transform.position;

                            sparksAnimationObject.SetActive(false);
                            sparksAnimationObject.SetActive(true);
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && canAttack)
        {
            bool attackPerformed = specialAttackScript.TryPerformSpecialAttack();
            if (attackPerformed)
            {
                animator.SetTrigger("AtaqueEspecial");
                attackCooldownCoroutine = StartCoroutine(AttackCooldown());
            }
        }
    }

    private void PerformAttack(Vector2 direction)
    {
        Vector2 attackPoint = (Vector2)transform.position + direction.normalized * attackRange / 2;


        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint, attackRange, LayerMask.GetMask("Enemy", "Projectile"));
        foreach (var hit in hitObjects)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyMovementAndHealth enemy = hit.GetComponent<EnemyMovementAndHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);


                    GameObject sparksInstance = Instantiate(sparksAnimationObject, enemy.transform.position, Quaternion.identity);
                    sparksInstance.SetActive(true);
                    Destroy(sparksInstance, attackParticules);
                }
            }
            else if (hit.CompareTag("Projectile"))
            {
                Destroy(hit.gameObject);
            }
        }
    }

    void SetAttackAnimationParameters(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetFloat("AttackHorizontal", Mathf.Sign(direction.x));
            animator.SetFloat("AttackVertical", 0);
            lastAttackHorizontal = Mathf.Sign(direction.x);
            lastAttackVertical = 0;
        }
        else
        {
            animator.SetFloat("AttackHorizontal", 0);
            animator.SetFloat("AttackVertical", Mathf.Sign(direction.y));
            lastAttackHorizontal = 0;
            lastAttackVertical = Mathf.Sign(direction.y);
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        canAttack = true;
    }
    void OnDestroy()
    {
        if (attackCooldownCoroutine != null)
        {
            StopCoroutine(attackCooldownCoroutine);
        }
    }

    public void IncreaseDamage(int amount)
    {
        damageAmount += amount;
    }
}