using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// PC의 행동 필드 정의
    /// </summary>
    [Header("Movement")]
    public float jumpForce = 10f; // 점프 힘
    private int jumpCount = 0; // 점프 횟수
    public float fallMultiplier = 6f; // 낙하 가속도 배율
    private bool isGrounded;

    /// <summary>
    /// 일반 필드 정의
    /// </summary>
    private Rigidbody2D rb;
    private bool isExiting = false; // 종료 애니메이션 여부
    public float exitSpeed = 5f; // 종료 시 이동 속도

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 6f; // 중력 설정
    }

    // Update is called once per frame
    void Update()
    {
        // 점프 입력 처리(스페이스바)
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCount < 2) // 가변점프 허용 안함 && 2단 점프 허용
        {
            Jump();
        }
        // 낙하 가속도 증가 처리
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
    // 점프 처리 메서드
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); // 빠른 상승
        jumpCount++;
    }
    // 충돌 처리 메서드
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 지면과 충돌
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; // 지면에 닿으면 점프 횟수 초기화
        }
        // falling off the platform
        if (collision.gameObject.CompareTag("FallZone"))
        {
            transform.position = new Vector2(-7f, 0.5f); // 떨어졌을 시, 복귀
            rb.linearVelocity = Vector2.zero; // 속도 초기화
        }
    }

    // 플레이어 종료 애니메이션 시작 메서드
    public void StartExitAnimation()
    {
        isExiting = true;
        rb.linearVelocity = new Vector2(exitSpeed, rb.linearVelocity.y); // 물리 초기화
    }

    void FixedUpdate()
    {
        if (isExiting)
        {
            rb.linearVelocity = new Vector2(exitSpeed, rb.linearVelocity.y); // 오른쪽으로 이동
        }

        if (transform.position.x > 10f)
        {
            isExiting = false; // 화면 밖으로 나가면 종료 애니메이션 중지

            Object.FindAnyObjectByType<StageManager>().SwitchToBossBattle();
            gameObject.SetActive(false); // 플레이어 비활성화
        }
    }
}
