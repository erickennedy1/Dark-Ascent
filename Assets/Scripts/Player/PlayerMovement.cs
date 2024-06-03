using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configura��es de movimento")]
    public float moveSpeed = 5f;
    private float dashDistance = 3f;
    private float dashDuration = 0.1f;
    public bool canMove = true;

    private Vector2 movement;
    public bool isMoving = false;
    private float ultimoMovimentoHorizontal = 0f;
    private float ultimoMovimentoVertical = -1f;
    private bool isDashing = false;
    private float lastDashTime = 0f;

    private PlayerAttack playerAttack;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerMana playerMana;
    private DashCooldownUI dashCooldownUI;
    private bool isCooldown = false;

    void Start()
    {
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMana = GetComponent<PlayerMana>();

        dashCooldownUI = FindObjectOfType<DashCooldownUI>();
    }

    void Update()
    {
        if (canMove && !isDashing)
        {
            HandleMovementInput();
            if (Input.GetKeyDown(KeyCode.Space) && !isCooldown) // Verifica se não está em cooldown
            {
                if (playerMana.currentMana >= 10)
                {
                    TryDash();
                }
            }
        }
        else
        {
            SetMovementZero();
        }
    }

    public void SetMovementZero()
    {
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
        animator.SetBool("IsMoving", false);
        movement = Vector2.zero;
        if(isMoving){
            isMoving = false;
            SoundManager.Instance.StopSoundLoop("Player_Movement");       
        }
    }

    void HandleMovementInput()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(moveHorizontal, moveVertical).normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        //Estava parado e andou
        if(!isMoving && movement.magnitude > 0){
            isMoving = true;
            animator.SetBool("IsMoving", true);
            SoundManager.Instance.PlaySound("Player_Movement");
        }
        //Estava andando e parou
        else if(isMoving && movement.magnitude == 0){
            isMoving = false;
            animator.SetBool("IsMoving", false);
            SoundManager.Instance.StopSoundLoop("Player_Movement");
        }

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
        dashCooldownUI.StartCooldown();
        StartCoroutine(DashCooldown());
    }

    IEnumerator Dash()
    {
        SoundManager.Instance.PlaySound("Player_Dash");
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

    IEnumerator DashCooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(2f); 
        isCooldown = false;
    }
}
