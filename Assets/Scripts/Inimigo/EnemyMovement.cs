using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Configurações de movimentação")]
    public float speed = 3f;
    public float followDistance = 5f;

    private Transform player;
    private Animator animator;
    private EnemyHealth enemyHealth;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (player == null || enemyHealth.IsKnockedBack)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= followDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            if (player.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            animator.SetBool("Movimentando", true);
        }
        else
        {
            animator.SetBool("Movimentando", false);
        }
    }
}
