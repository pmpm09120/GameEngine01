
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider2D;
    private Vector2 movement;
    private Vector2 inputDir;

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

    public float endWorldY = -50f; //-y좌표 끝

    [Header("Wall Check")]
    public LayerMask wallLayer;
    public float raycastDis;
    public Transform checkPoint;
    public Vector2 boxSize;

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

        //현재 씬 재로드
        if (rb.position.y <= endWorldY)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        RaycastHit2D hit = Physics2D.BoxCast(checkPoint.position, boxSize, checkPoint.eulerAngles.z, inputDir, raycastDis, wallLayer);
        if (hit.collider != null)
        {
            inputDir.x = 0;
        }

        rb.linearVelocity = new Vector2(inputDir.x * movespeed, rb.linearVelocity.y);
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
        inputDir = new Vector2(movement.x, movement.y);

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

        //BoxCast DRAW
        if (checkPoint == null) return;

        // 1. 박스가 시작하는 위치 그리기
        Gizmos.color = Color.yellow;
        DrawBox(checkPoint.position, boxSize, checkPoint.eulerAngles.z);

        // 2. 박스가 이동한 끝 지점(MaxDistance) 위치 그리기
        Vector2 endPosition = (Vector2)checkPoint.position + (inputDir.normalized * raycastDis);
        Gizmos.color = Color.red;
        DrawBox(endPosition, boxSize, checkPoint.eulerAngles.z);

        // 3. 시작점과 끝점을 연결하는 선 그리기 (궤적 표현)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(checkPoint.position, endPosition);
    }

    // 회전각이 포함된 2D 박스를 Gizmos로 그려주는 보조 함수
    private void DrawBox(Vector2 center, Vector2 size, float rotAngle)
    {
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, rotAngle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = Matrix4x4.identity; // 매트릭스 초기화
    }


    ///목표 시간 도달 방식(Accumulated/Target Time)'과 '남은 시간 차감 방식(Countdown)
    /// 1 ----------------------------------------
    /// 게임 시간 경과 + 고정시간과 현재 게임시간의 비교
    /// float timescale
    /// float Nexttimescale
    /// 
    /// if(Time.time > Nexttimescale )
    /// {
    ///     Debug.Log("true");
    ///     Use();
    /// }
    /// else
    /// {
    ///     Debug.Log("false");
    /// }
    /// 
    /// void Use()
    /// {
    ///     Nexttimescale = Time.time + timescale;
    /// }
    /// 
    /// 2 --------------------------------------------
    /// 동적 시간값이 0 이상이라면 시간에 따른 차감
    /// float timescale
    /// float currenttimescale
    /// 
    /// if(currenttimescale > 0)
    /// {
    ///     Debug.Log("true");
    ///     currenttimescale -= Time.deltaTime;
    /// }
    /// else
    /// {
    ///     Debug.Log("false");
    ///     currenttimescale = timescale
    /// }
    /// 
}
