using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configura��es de movimento")]
    public float moveSpeed = 5f;
    private float dashDistance = 3f;
    private float dashDuration = 0.1f;
    private float dashCooldown = 0.2f;
    public bool canMove = true;

    private Vector2 movement;
    private float ultimoMovimentoHorizontal = 0f;
    private float ultimoMovimentoVertical = -1f;
    private bool isDashing = false;
    private float lastDashTime = 0f;

    private PlayerAttack playerAttack;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMana playerMana; 

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMana = GetComponent<PlayerMana>(); 
    }

    void Update()
    {
        if (canMove && !isDashing)
        {
            HandleMovementInput();
            if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
            {
                if (playerMana.currentMana >= 10)
                {
                    TryDash();
                }
            }
        }else{
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 0);
            animator.SetBool("IsMoving", false);
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

        UpdateLastMovementDirection();

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateLastMovementDirection()
    {
        if (movement.magnitude > 0 || playerAttack.acabouDeAtacar)
        {
            ultimoMovimentoHorizontal = movement.magnitude > 0 ? movement.x : playerAttack.ultimoAtaqueHorizontal;
            ultimoMovimentoVertical = movement.magnitude > 0 ? movement.y : playerAttack.ultimoAtaqueVertical;

            animator.SetFloat("LastHorizontal", ultimoMovimentoHorizontal);
            animator.SetFloat("LastVertical", ultimoMovimentoVertical);

            if (playerAttack.acabouDeAtacar)
                playerAttack.acabouDeAtacar = false;
        }
    }

    void TryDash()
    {
        playerMana.UsarMana(10); 
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        lastDashTime = Time.time;
        isDashing = true;
        animator.SetBool("IsDashing", true);

        Vector2 dashDirection = new Vector2(ultimoMovimentoHorizontal, ultimoMovimentoVertical).normalized;
        animator.SetFloat("DashHorizontal", dashDirection.x);
        animator.SetFloat("DashVertical", dashDirection.y);

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashDistance * Time.fixedDeltaTime / dashDuration);
            yield return null;
        }

        isDashing = false;
        animator.SetBool("IsDashing", false);
        animator.SetFloat("DashHorizontal", 0);
        animator.SetFloat("DashVertical", 0);
    }
}
