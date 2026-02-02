using UnityEngine;
using System.Collections;

public class BossAI_Whale : MonoBehaviour
{
    /// <summary>
    /// 보스 스탯
    /// </summary>
    [Header("Stats")]
    public float maxHP = 500f;// 체력
    private float currentHP; // 현재 체력
    public float moveRange = 3f; // 움직일 범위
    public float moveSpeed = 2f; // 움직일 속도
    private Vector3 startPosition; // 시작 위치

    /// <summary>
    /// 보스 공격
    /// </summary>
    [Header("Attack Settings")]
    public GameObject bossBulletPrefab; // 기본 탄환
    public Transform shootPoint_bullet; // 탄환 발사 위치
    public float fireInterval = 1.5f; // 탄환 발사 간격
    public GameObject warningZonePrefab; // 경고 존 프리팹
    public GameObject missilePrefab; // 미사일 프리팹
    public float missileSpeed = 25f; // 미사일 속도
    public GameObject mobPrefab; // 소환 몹 프리팹
    public int maxMobCount = 2; // 최대 소환 몹 수

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
    /// HP UI
    /// </summary>
    [Header("HP UI")]
    public HP_Bar hpBar; // Boss HP UI 연결

    /// <summary>
    /// 패배씬 세팅
    /// </summary>
    [Header("DefeatSettings")]
    public float targetY; // 가라앉는 위치
    public float sinkSpeed; // 가라앉는 속도
    public float explosionRepeatCount; // 폭발 애니메이션 반복 횟수
    public GameObject defeatEffectPrefab; // 패배 이펙트를 담은 부모 프리팹
    public WorldScroller worldScroller1; // 배경을 멈추기 위한 참조
    public WorldScroller worldScroller2; // 배경을 멈추기 위한 참조

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // 현재 스프라이트 컬러 저장
    }

    void Start()
    {
        currentHP = maxHP;
        startPosition = transform.position; // 시작 위치 저장
        InvokeRepeating("Pattern1", 2f, fireInterval); // 패턴1 시작
        StartCoroutine(PatternManager()); // 패턴 관리

    }

    void Update()
    {
        // 체력이 0 초과일 때 움직임
        if (currentHP > 0)
        {
            float newY = startPosition.y + Mathf.Sin(Time.time * moveSpeed) * moveRange;
            transform.position = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }

    // 패턴 관리 코루틴
    IEnumerator PatternManager()
    {
        while (currentHP > 0)
        {
            float hpPercent = currentHP / maxHP;

            if (hpPercent <= 0.7f)
            {
                int randomPattern = Random.Range(0, 2); // 0 또는 1 선택
                if (randomPattern == 0) StartMissileAttack();
                else SpawnMob();

                yield return new WaitForSeconds(4f); // 패턴 간 대기 시간
            }
            yield return new WaitForSeconds(1f); // 기본 대기 시간
        }
    }

    // 패턴1: 기본 탄환 발사
    void Pattern1()
    {
        Instantiate(bossBulletPrefab, shootPoint_bullet.position, Quaternion.identity); // 플레이어 방향으로 탄환 발사
    }

    // 패턴2: 미사일 공격
    public void StartMissileAttack()
    {
        StartCoroutine(MissileSequence());
    }

    // 패턴3: 소환 몹 생성
    public void SpawnMob()
    {
        int currentMobCount = GameObject.FindGameObjectsWithTag("Mob").Length; // 현재 소환된 몹 수 확인
        if (currentMobCount < maxMobCount)
        {
            Vector3 spawnPosition = shootPoint_bullet.position; // 소환 위치 설정
            GameObject mob = Instantiate(mobPrefab, spawnPosition, Quaternion.identity); // 몹 소환
            
            Rigidbody2D mobRb = mob.GetComponent<Rigidbody2D>();
            if (mobRb != null)
            {
                mobRb.linearVelocity = Vector2.left * 2f; // 몹 이동 속도 설정
            }
        }
    }

    // 미사일 공격 시퀀스
    IEnumerator MissileSequence()
    {
        // 공격 위치 결정
        float targetY = GameObject.FindWithTag("Player").transform.position.y;
        Vector3 spawnPos = new Vector3(0, targetY, 0);

        // Warning Zone 생성
        GameObject warning = Instantiate(warningZonePrefab, spawnPos, Quaternion.identity);
        warning.transform.localScale = new Vector3(30f, 1f, 1f); // 경고 존 크기 조절

        yield return new WaitForSeconds(1.5f); // 경고 존 표시 시간

        // 경고창 제거 및 미사일 발사
        Destroy(warning);
        LaunchMissile(targetY);
    }

    // 미사일 발사
    void LaunchMissile(float yPos)
    {
        // 보스 현재 위치에서 미사일 발사
        Vector3 missileSpawnPos = new Vector3(transform.position.x, yPos, 0);
        GameObject missile = Instantiate(missilePrefab, missileSpawnPos, Quaternion.identity); // 미사일 생성

        missile.GetComponent<EnemyBullet>().speed = missileSpeed; // 미사일 속도 설정
    }

    // 보스 데미지 처리
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        //Debug.Log($"Boss HP: {currentHP}/{maxHP}");

        // hp UI 로직
        if (hpBar != null)
        {
            hpBar.UpdateHP(currentHP, maxHP);
        }

        // 피격 이팩트 로직
        if (hitCoruotine != null) StopCoroutine(hitCoruotine);
        hitCoruotine = StartCoroutine(HitFlashRoutine());

        // 보스 사망
        if (currentHP <= 0)
        {
            BossDeath();
        }
    }

    // 이팩트 코루틴
    IEnumerator HitFlashRoutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    // 보스 사망 처리
    void BossDeath()
    {
        CancelInvoke(); // 모든 패턴 중지
        Debug.Log("Boss Defeated!");
        StartCoroutine(DefeatRoutione()); // 패배씬 코루틴 시작
    }

    // 패배씬 코루틴
    IEnumerator DefeatRoutione()
    {
        // 배경 멈춤
        worldScroller1.enabled = false;
        worldScroller2.enabled = false;

        // 소환된 모든 Mob 제거
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob");
        foreach (GameObject mob in mobs)
        {
            MobAI mobAI = mob.GetComponent<MobAI>();
            if (mobAI != null)
            {
                mobAI.StartCoroutine("DieRoutine");
            }
        }

        if (defeatEffectPrefab != null)
        {
            if (GetComponent<Collider2D>()) GetComponent<Collider2D>().enabled = false; // 물리 충돌 끄기

            if (defeatEffectPrefab != null)
            {
                GameObject effectGroup = Instantiate(defeatEffectPrefab, transform.position, Quaternion.identity);
                effectGroup.transform.SetParent(this.transform);

                Animator[] childAnims = effectGroup.GetComponentsInChildren<Animator>(); // 자식의 모든 애니메이터 가져온 리스트

                if (childAnims.Length > 0)
                {
                    for (int i = 0; i < explosionRepeatCount; i++)
                    {
                        foreach (var anim in childAnims)
                        {
                            anim.Play("ExplosionEffect", 0, 0f); // 폭발 이팩트 재생
                        }
                        // 첫 번째 자식 애니메이션 길이를 기준으로 대기
                        yield return new WaitForSeconds(childAnims[0].GetCurrentAnimatorStateInfo(0).length); // 애니메이션 한 번의 길이를 기다림
                    }
                }

                foreach (var anim in childAnims)
                {
                    anim.SetTrigger("ExplosionEnd"); // 연기 상태로 전이
                }
            }
        }

        // 가라앉기
        while (transform.position.y > targetY)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                transform.position.x, targetY, transform.position.z), sinkSpeed * Time.deltaTime);
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime); // 가라 앉기
            yield return null;
        }

        // 승리 로직
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.FinalizeReward(true);
        }
        else
        {
            Debug.Log("Couldnt find RewardManager");
        }
    }
}
