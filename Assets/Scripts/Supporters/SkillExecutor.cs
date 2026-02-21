using System.Collections;
using UnityEngine;
using System.Collections.Generic;

// ID에 해당하는 스킬을 수행
public class SkillExecutor : MonoBehaviour
{
    public static SkillExecutor Instance;

    public Transform Player; // 플레이어 위치 참조
    
    // 1번 서포터 세팅
    [Header("Supporter1 Settings")]
    public GameObject TurretPrefab; // ID 1 포탑

    // 4번 서포터 세팅
    [Header("Supporter4 Settings")]
    public GameObject DrillPrefab; // ID 4 드릴

    private bool isLifeStealActive = false; // 2번 서포터 액티브 스킬 사용중 상태 여부
    private float lifeStealPercent; // 2번 서포터 액티브 스킬 힐 퍼센트
    private float currentDmgBonus = 0f; // 4번 서포터 대미지 관리

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
    /// 3번: 플레이어 기준 일자 범위에 300% 대미지 1회 + 영역 제외 나머지 모든 영역에 30% 대미지 1회
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
            Player.GetComponent<PlayerHealth>().Heal(Mathf.RoundToInt(healAmount));
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
        float mainDmgPercent = 3.0f + (lv - 1) * 0.5f; // 레벨에 따라 50%씩 증가
        float subDmgPercent = 0.3f + (lv - 1) * 0.1f; // 레벨에 따라 10%씩 증가

        //  현재 플레이어 대미지
        int atkLevel = PlayerPrefs.GetInt("Upgrade_ATK_Level", 1);
        float playerBaseDmg = 5 + (atkLevel - 1) * 5;

        float mainFinalDmg = playerBaseDmg * mainDmgPercent; // 주 대미지
        float subFinalDmg = playerBaseDmg * subDmgPercent; // 주영역 외 대미지

        // 메인  대미지 판정
        Collider2D[] lineHits = Physics2D.OverlapBoxAll(Player.position, new Vector2(20f, 2f), 0f);
        HashSet<GameObject> mainHitTargets = new HashSet<GameObject>(); // 메인 대미지 입을 적들
        
        foreach (var hit in lineHits)
        {
            if(hit.CompareTag("Mob") || hit.CompareTag("Boss"))
            {
                ApplyDamage(hit.gameObject, mainFinalDmg);
                mainHitTargets.Add(hit.gameObject); // 일자 영역에 맞은 적 저장
            }
        }

        // 맵 전체 적에게 보조 대미지
        GameObject[] allMobs = GameObject.FindGameObjectsWithTag("Mob");
        GameObject[] allBosses = GameObject.FindGameObjectsWithTag("Boss");

        ProcessSubDamage(allMobs, mainHitTargets, subFinalDmg);
        ProcessSubDamage(allBosses, mainHitTargets, subFinalDmg);
    }

    /// <summary>
    /// 3번 서포터 대미지 적용 메서드
    /// </summary>
    /// <param name="target">대상</param>
    /// <param name="dmg">대미지</param>
    void ApplyDamage(GameObject target, float dmg)
    {
        // 인터페이스 가진 컴포넌트에게 대미지
        IDamageable damageable = target.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(dmg);
        }
    }

    /// <summary>
    /// 3번 서포터 메인 영역 이외의 영역에 대미지를 주는 메서드
    /// </summary>
    /// <param name="targets">대상</param>
    /// <param name="mainHits">메인 대미지를 입은 오브젝트 셋</param>
    /// <param name="dmg">대미지</param>
    void ProcessSubDamage(GameObject[] targets, HashSet<GameObject> mainHits, float dmg)
    {
        foreach (var obj in targets)
        {
            if (!mainHits.Contains(obj))
            {
                ApplyDamage(obj, dmg);
            }
        }
    }

    /// <summary>
    /// ID 3 서포터 패시브 스킬 메서드
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    public void StartPassive_ID3(int lv)
    {
        StartCoroutine(PassiveRoutine_ID3(lv));
    }

    /// <summary>
    /// ID 3 서포터 패시브로 힐을 하는 코루틴
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    /// <returns>10초마다</returns>
    IEnumerator PassiveRoutine_ID3(int lv)
    {
        float healAmount = 2f + (lv - 1) * 1f; // 패시브: 2 + (레벨-1)
        while (true)
        {
            yield return new WaitForSeconds(10f);
            Player.GetComponent<PlayerHealth>().Heal(Mathf.RoundToInt(healAmount));
            Debug.Log($"ID 3 passive: {healAmount} heal");
        }
    }

    /// <summary>
    /// 4번 서포터 액티브 스킬
    /// </summary>
    /// <param name="lv">스킬 레벨</param>
    /// <returns></returns>
    IEnumerator Active_ID4(int lv)
    {
        float bonusRatio = 0.1f + (lv - 1) * 0.02f;

        var controller = Player.GetComponent<BossPlayerController>();
        float originalSpeed = controller.moveSpeed;

        controller.moveSpeed *= (1f + bonusRatio); // 속도 증가
        currentDmgBonus = bonusRatio; // 공격력 보너스 설정

        // 드릴 생성
        GameObject drill = Instantiate(DrillPrefab, Player.position + Vector3.right * 1.5f, Quaternion.identity, Player);
        drill.GetComponent<Drill>().Init(lv, bonusRatio);

        // PlayerHealth의 무적 메서드 호출 (아래 2번 항목에서 PlayerHealth 수정)
        Player.GetComponent<PlayerHealth>().SetTemporaryInvincible(10f);

        yield return new WaitForSeconds(10f);

        // 원상 복구
        controller.moveSpeed = originalSpeed;
        currentDmgBonus = 0f;
        if (drill != null) Destroy(drill);
    }

    /// <summary>
    /// 4번 서포터 보너스 대미지 계산 위한 메서드
    /// </summary>
    /// <returns>currentDmgBonus</returns>
    public float GetCurrentDmgBonus() => currentDmgBonus;
}
