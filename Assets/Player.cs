
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;
    private Vector2 movement;

    [Header("PlayerSettings")]
    public float movespeed = 8;
    public float jumpPower = 6;

    [Header("Jump Buffer / GroundTimer")]
    public float jumpbufferTime = 0.12f;
    public float jumpbufferCounter;
    public float groundTime = 0.12f;
    public float groundTimeCounter;

    [Header("Ground Check")]
    public bool isJump = false;
    public bool isGrounded = false;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;
    //RayCast

    [Header("CustomGravity")]
    public float defaultGravityScale = 1;
    public float FallGravity = 1.2f;
    public float apexGravityScale = 0.5f;
    public void OnMove(InputAction.CallbackContext callback)
    {
        movement = callback.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext callback)
    {
        if (callback.performed)
        {
            isJump = true;
        }
        else if (callback.canceled)
        {
            isJump = false;
        }
    }
    private void Update()
    {
        PlayerControl();
        TryJump();
        ApplyCustomGravity();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        rb.linearVelocity = new Vector2(movement.x * movespeed, rb.linearVelocity.y);
    }

    void TryJump()
    {
        if (groundTimeCounter > 0f && jumpbufferCounter >0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);

            groundTimeCounter = 0;
            jumpbufferCounter = 0;
        }
    }

    void PlayerControl()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        //CoyoteTime
        if (isGrounded)
        {
            groundTimeCounter = groundTime;
        }
        else //땅 위에 있지 않은 경우 땅 위에 있는 판정 시간
        {
            groundTimeCounter -= Time.deltaTime;
        }

        //JumpBuffer
        if (isJump)
        {
            jumpbufferCounter = jumpbufferTime;
        }
        else //점프를 빠르게 눌렀을 경우 점프 판정이 허용되는 시간
        {
            jumpbufferCounter -= Time.deltaTime;
        }
    }

    private void ApplyCustomGravity()
    {
        //낙하중인 경우
        if (rb.linearVelocity.y < 0f)
        {
            rb.gravityScale = defaultGravityScale * FallGravity;
        }
        //공중의 정점인 경우
        else if (rb.linearVelocity.y > 0f && Mathf.Abs(rb.linearVelocity.y) < 2f)
        {
            rb.gravityScale = defaultGravityScale * apexGravityScale;
        }
        //땅 위 인 경우
        else
        {
            rb.gravityScale = defaultGravityScale;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
