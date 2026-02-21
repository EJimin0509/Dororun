using System.Collections;
using UnityEngine;

public class MobAI : MonoBehaviour, IDamageable
{
    public float moveSpeed = 3f; // 이동 속도
    public float shootingInterval = 2f; // 탄환 발사 간격
    public GameObject bulletPrefab;// 탄환 프리팹
    private Transform playerTransform; // 플레이어 위치 참조
    public float maxHP = 20f;// 최대 체력
    private float currentHP; // 현재 체력
    public HP_Bar mobHPUI; // HP UI 참조

    /// <summary>
    /// 피격 이펙트
    /// </summary>
    [Header("Hit Effect")]
    public float flashDuration = 0.1f; // 이펙트 시간
    public Color hitColor; // 피격 색
    private SpriteRenderer spriteRenderer; // 스프라이트 참조
    private Color originalColor; // 원래 색
    private Coroutine hitCoruotine; // 이팩트 코루틴

    /// <summary>
    /// 죽을 시 폭발 셋팅
    /// </summary>
    [Header("Die Animaion Settings")]
    public float explosionDelay; // 폭발 애니메이션 표시 시간

    private Animator anim; // 애니메이션 참조
    private Collider2D col; // 콜라이더
    private bool isExploding = false; //폭발 여부 체크, 정지를 위함
    private int explodeTriggerID; // IsDead 트리거를 위한 ID

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        explodeTriggerID = Animator.StringToHash("IsDead");

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(!isExploding) InvokeRepeating("ShootAtPlayer", 1f, shootingInterval); // 일정 간격으로 탄환 발사
        Destroy(gameObject, 10f); // 제거
    }


    void Update()
    {
        if (playerTransform != null) return;

        if(!isExploding)
        {
            Vector2 targetPos = new Vector2(transform.position.x - 1f, playerTransform.position.y); // 플레이어의 y좌표를 따라 이동
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime); // 이동
        }
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
        mobHPUI.UpdateHP(currentHP, maxHP);

        // 피격 이팩트 로직
        if (hitCoruotine != null) StopCoroutine(hitCoruotine);
        hitCoruotine = StartCoroutine(HitFlashRoutine());

        if (currentHP <= 0)
        {
            StartCoroutine(DieRoutine()); // 사망 루틴
        }
    }

    // 이팩트 코루틴
    IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    IEnumerator DieRoutine()
    {
        isExploding = true;
        mobHPUI.gameObject.SetActive(false); // UI 제거

        if (col != null) col.enabled = false; // 통과 가능하게

        if (anim != null)
        {
            anim.SetTrigger(explodeTriggerID);
            Debug.Log("Explosion!");
        }

        yield return new WaitForSeconds(explosionDelay);
        Destroy(gameObject); // 몹 제거
        isExploding = false;
    }
}
