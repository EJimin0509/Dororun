using System.Collections;
using UnityEngine;

// ID에 해당하는 스킬을 수행
public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance;

    public Transform Player; // 플레이어 위치 참조
    // 1번 서포터 세팅
    [Header("Supporter1 Settings")]
    public GameObject TurretPrefab; // ID 1 포탑

    private bool isLifeStealActive = false; // 2번 서포터 액티브 스킬 사용중 상태 여부
    private float lifeStealPercent; // 2번 서포터 액티브 스킬 힐 퍼센트

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 레벨에 따른 수치 계산용 헬퍼 메서드
    /// </summary>
    /// <param name="id">서포터 ID</param>
    /// <returns></returns>
    int GetLv(int id) => PlayerPrefs.GetInt($"Upgrade_Supporter_{id}_Level", 1);

    /// <summary>
    /// 스킬 수행 메서드
    /// 1번: 플레이어 위 아래에 적 자동 공격하는 포탑 2개 생성(5초)
    /// 2번: 플레이어가 가한 모든 대미지만큼 HP 회복(5초)
    /// 3번: 플레이어 기준 일자 범위에 300% 대미지 1회 + 영역 제외 나머지 모든 영역에 30% 대미지 1회 + 모든 적 탄환 제거
    /// 4번: 플레이어 스피드 10% 상승 + 공격력 10% 상승 + 플레이어 앞에 드릴 생성 + 플레이어 무적 상태 + 충돌한 모든 적 대미지 + 충돌한 투사체 삭제(10초)
    /// </summary>
    /// <param name="id">서포터 ID</param>
    public void ExecuteActive(int id)
    {
        int lv = GetLv(id);
        switch (id)
        {
            case 1:
                // id 1 액티브 스킬
                StartCoroutine(Active_ID1(lv));
                break;
            case 2:
                StartCoroutine(Active_ID2(lv));
                // id 2 액티브 스킬
                break;
            case 3:
                // id 3 액티브 스킬
                ExecuteActive_ID3(lv);
                break;
            case 4:
                // id 4 액티브 스킬
                StartCoroutine(Active_ID4(lv));
                break;
        }
    }


    /// <summary>
    /// 1번 서포터 액티브 스킬
    /// 터렛 생성
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    /// <returns></returns>
    IEnumerator Active_ID1(int lv)
    {
        // 플레이어 위 아래에 포탑 2개 생성
        Vector3 pos1 = Player.position + new Vector3(1f, 1.5f, 0);
        Vector3 pos2 = Player.position + new Vector3(1f, -1.5f, 0);

        GameObject t1 = Instantiate(TurretPrefab, pos1, Quaternion.identity, Player);
        GameObject t2 = Instantiate(TurretPrefab, pos2, Quaternion.identity, Player);

        // 터렛 대미지 전달
        t1.GetComponent<Turret>().Init(lv);
        t2.GetComponent<Turret>().Init(lv);

        yield return new WaitForSeconds(5f);

        Destroy(t1);
        Destroy(t2);
    }

    /// <summary>
    /// 2번 서포터 액티브 스킬
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    /// <returns></returns>
    IEnumerator Active_ID2(int lv)
    {
        isLifeStealActive = true;
        lifeStealPercent = 0.03f + (lv - 1) * 0.03f; // 레벨에 따라 3% 씩 증가
        yield return new WaitForSeconds(5f);

        isLifeStealActive = false;
    }

    /// <summary>
    /// 2번 서포터 액티브 스킬
    /// 대미지의 퍼센테이지 만큼 회복
    /// </summary>
    /// <param name="dmg">대미지</param>
    public void ReportDamage(float dmg)
    {
        if (isLifeStealActive)
        {
            float healAmount = dmg * lifeStealPercent;
            Player.GetComponent<PlayerHealth>().Heal(healAmount);
        }
    }

    /// <summary>
    /// 3번 서포터 액티브 스킬
    /// 일직선 범위에 큰 대미지, 주위 전체에 대미지 가함
    /// 모든 적 총알 제거
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    public void ExecuteActive_ID3(int lv)
    {
        float mainDmgRatio = 3.0f + (lv - 1) * 0.5f; // 레벨에 따라 50%씩 증가
        float subDmgRatio = 0.3f + (lv - 1) * 0.1f; // 레벨에 따라 10%씩 증가

        // 탄환 제거
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach(var b in bullets)
        {
            if (b.GetComponent<EnemyBullet>() != null) Destroy(b);
        }

        // 대미지 판정
        Collider2D[] lineHit = Physics2D.OverlapBoxAll(Player.position, new Vector2(20f, 2f), 0f);
        foreach(var hit in lineHit)
        {
            if(hit.CompareTag("Mob") || hit.CompareTag("Boss"))
            {
                // 대미지 입히는 로직 필요
            }
        }
    }

    /// <summary>
    /// 4번 서포터 액티브 스킬
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    /// <returns></returns>
    IEnumerator Active_ID4(int lv)
    {
        float bonusStat = 0.1f + (lv - 1) * 0.02f; // 레벨에 따라 2%씩 증가

        // 보너스 스탯 적용 로직

        yield return new WaitForSeconds(10f);

        // 복구 로직
    }
}
