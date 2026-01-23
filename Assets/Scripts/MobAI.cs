using System.Collections;
using UnityEngine;

public class MobAI : MonoBehaviour
{
    public float moveSpeed = 3f; // 이동 속도
    public float shootingInterval = 2f; // 탄환 발사 간격
    public GameObject bulletPrefab;// 탄환 프리팹
    private Transform playerTransform; // 플레이어 위치 참조
    public float maxHP = 20f;// 최대 체력
    private float currentHP; // 현재 체력
    private SpriteRenderer spriteRenderer; // 스프라이트 렌더러 참조
    private Color originalColor; // 원래 색상 저장

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("ShootAtPlayer", 1f, shootingInterval); // 일정 간격으로 탄환 발사
        Destroy(gameObject, 10f); // 5초 후 제거
    }


    void Update()
    {
        if (playerTransform != null) return;

        Vector2 targetPos = new Vector2(transform.position.x - 1f, playerTransform.position.y); // 플레이어의 y좌표를 따라 이동
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime); // 이동

    }

    void ShootAtPlayer()
    {
        if (playerTransform == null) return;
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Vector2 direction = (playerTransform.position - bullet.transform.position).normalized; // 플레이어 방향 계산

        bullet.GetComponent<EnemyBullet>().SetDirection(direction); // 탄환 방향 설정
    }

    // 피격 처리 메서드
    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        // 피격 효과
        StopCoroutine("HitFlash");
        StartCoroutine("HitFlash");

        if(currentHP <= 0)
        {
            Die(); // 체력이 0 이하가 되면 몹 제거
        }
    }

    IEnumerable HitFlash()
    {
        spriteRenderer.color = Color.red; // 피격 시 빨간색으로 변경
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor; // 원래 색상으로 복원
    }

    void Die()
    {
        // 사망 이펙트 추가 가능

        Destroy(gameObject); // 몹 제거
    }
}
