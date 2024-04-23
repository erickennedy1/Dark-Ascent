using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float moveSpeed = 5f;

    private Vector2 movement;
    private float lastHorizontal = 0f;
    private float lastVertical = -1f;

    private PlayerAttack playerAttack;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>(); 
    }

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        if (objs.Length > 1)
        {
            Debug.Log("Destroy, Player > 1: " + objs.Length);
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this);
    }
    void FixedUpdate()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("IsMoving", movement.magnitude > 0);

        if (movement.magnitude > 0 || playerAttack.justAttacked)
        {
            lastHorizontal = movement.magnitude > 0 ? movement.x : playerAttack.lastAttackHorizontal;
            lastVertical = movement.magnitude > 0 ? movement.y : playerAttack.lastAttackVertical;

            if (playerAttack.justAttacked)
            {
                animator.SetFloat("LastHorizontal", playerAttack.lastAttackHorizontal);
                animator.SetFloat("LastVertical", playerAttack.lastAttackVertical);
                playerAttack.justAttacked = false;
            }
        }
        else
        {
            animator.SetFloat("LastHorizontal", lastHorizontal);
            animator.SetFloat("LastVertical", lastVertical);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
