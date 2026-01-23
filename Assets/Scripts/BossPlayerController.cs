using UnityEngine;
using UnityEngine.InputSystem;

public class BossPlayerController : MonoBehaviour
{
    /// <summary>
    /// Boss 씬 PC 이동
    /// </summary>
    [Header("Movement")]
    public float moveSpeed = 8f; // 이동속도
    private Rigidbody2D rb;

    /// <summary>
    /// Boss 씬 PC 공격
    /// </summary>
    [Header("Combat")]
    public GameObject bulletPrefab; // 총알 프리펩
    public Transform firePoint; // 총알 발사 위치
    public float fireRate = 0.3f; // 총알 발사 속도
    private float nextFireTime = 0.3f; // 발사 지연 시간


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 즉시 정지 입력을 위한 부분
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // 공격 입력 처리
        if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    // 8방향 이동 입력 처리
    private void FixedUpdate()
    {
        Vector2 moveInput = Vector2.zero;
        var keyboard = Keyboard.current;

        if (keyboard.wKey.isPressed) moveInput.y += 1;
        if (keyboard.sKey.isPressed) moveInput.y -= 1;
        if (keyboard.aKey.isPressed) moveInput.x -= 1;
        if (keyboard.dKey.isPressed) moveInput.x += 1;

        // 입력 없을 시, 즉시 정지
        if (moveInput != Vector2.zero)
        {
            rb.linearVelocity = moveInput.normalized * moveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        LimitPostion(); // 화면 밖으로 나가지 않도록 위치 제한
    }

    // 총알 발사 메서드
    void Shoot()
    {
        if(bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    // 화면 밖으로 나가지 않도록 위치 제한 메서드
    void LimitPostion()
    {
        // 카메라 뷰포트 경계 계산
        Vector2 minBound = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 maxBound = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // 플레이어 크기에 따른 패딩
        float paddingX = 0.5f;
        float paddingY = 0.5f;

        Vector3 currentPos = transform.position;

        // 경계 제한
        currentPos.x = Mathf.Clamp(currentPos.x, minBound.x + paddingX, maxBound.x - paddingX);
        currentPos.y = Mathf.Clamp(currentPos.y, minBound.y + paddingY, maxBound.y - paddingY);

        transform.position = currentPos;
    }
}
