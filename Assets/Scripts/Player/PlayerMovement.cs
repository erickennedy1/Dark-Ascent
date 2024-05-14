using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de movimento")]
    public float moveSpeed = 5f;
    public bool canMove = true;

    private Vector2 movement;
    private float ultimoMovimentoHorizontal = 0f;
    private float ultimoMovimentoVertical = -1f;

    private PlayerAttack playerAttack;
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>(); 
    }

    void FixedUpdate()
    {
        if (canMove)  
        {
            HandleMovementInput();
        }
    }

    void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetBool("IsMoving", movement.magnitude > 0);

        if (movement.magnitude > 0 || playerAttack.acabouDeAtacar)
        {
            ultimoMovimentoHorizontal = movement.magnitude > 0 ? movement.x : playerAttack.ultimoAtaqueHorizontal;
            ultimoMovimentoVertical = movement.magnitude > 0 ? movement.y : playerAttack.ultimoAtaqueVertical;

            if (playerAttack.acabouDeAtacar)
            {
                animator.SetFloat("LastHorizontal", playerAttack.ultimoAtaqueHorizontal);
                animator.SetFloat("LastVertical", playerAttack.ultimoAtaqueVertical);
                playerAttack.acabouDeAtacar = false;
            }
        }
        else
        {
            animator.SetFloat("LastHorizontal", ultimoMovimentoHorizontal);
            animator.SetFloat("LastVertical", ultimoMovimentoVertical);
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }


}
