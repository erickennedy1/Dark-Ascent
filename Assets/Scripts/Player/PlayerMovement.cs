using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float moveSpeed = 5f;

    [Header("Componentes")]
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 movement;
    private float lastHorizontal = 0f;
    private float lastVertical = -1f;

    private PlayerAttack playerAttack;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
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
