using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static int CurrentHP = 100; // 현재 HP, 공유를 위해 static
    public int maxHP = 100;

    /// <summary>
    /// PC의 상태 필드 정의
    /// </summary>
    [Header("Status")]
    public float invincibilityTime = 1.5f; // 무적 시간
    private bool isInvincible = false; // 무적 여부

    [Header("UI Reference")]
    public HP_Bar playerHPUI; // UI 연결

    /// <summary>
    /// 패배를 위한 세팅
    /// </summary>
    [Header("Defeated Setting")]
    public WorldScroller[] WorldScrollers; // 배경을 멈추기 위한 참조

    private Animator anim;
    private Rigidbody2D rb2d;
    private int isInvincibleID; // 애니메이션 bool 파라미터 IsInvincible을 위한 ID
    private int isDefeatedID; // 애니메이션 bool 파라미터 IsDefeated를 위한 ID

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        playerHPUI.UpdateHP(CurrentHP, maxHP);
        rb2d = GetComponent<Rigidbody2D>();

        isInvincibleID = Animator.StringToHash("IsInvincible");
        isDefeatedID = Animator.StringToHash("IsDefeated");

        // 레벨에 따른 HP 업데이트
        int hpLevel = PlayerPrefs.GetInt("Upgrade_HP_Level", 1);
        maxHP = 100 + (hpLevel - 1) * 50;
        CurrentHP = maxHP;

        // 스쿼드에 2번 서포터가 있다면  무적시간 증가
        bool hasID2 = PlayerPrefs.GetInt("Squad_Slot0", -1) == 2 || PlayerPrefs.GetInt("Squad_Slot1", -1) == 2;

        if (hasID2) invincibilityTime += 0.5f;
    }

    // 대미지 처리 메서드
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // 무적 중 실행하지 않음

        CurrentHP -= damage;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP);

        playerHPUI.UpdateHP(CurrentHP, maxHP); // UI 갱신
        Debug.Log($"dmg: {damage}, HP: {CurrentHP}/{maxHP}");

        if (anim != null && CurrentHP <= 0) Die(); // 사망

        if (anim != null && CurrentHP > 0) 
        {
            anim.SetBool(isInvincibleID, true);
            StartCoroutine(InvincibilityRoutine()); // 무적 상태
        }
    }

    // 사망 처리
    void Die()
    {
        StartCoroutine(DefeatedRoutine()); // 애니메이션 실행 코루틴
    }

    // 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !isInvincible)
        {
            TakeDamage(10); // Obstacle 충돌
        }
        else if (collision.gameObject.CompareTag("FallZone") && !isInvincible)
        {
            TakeDamage(10); // FallZone 충돌
        }
        else if (collision.gameObject.CompareTag("Bullet") && !isInvincible)
        {
            TakeDamage(10); // Bullet 충돌
        }
        else if (collision.gameObject.CompareTag("Missile") && !isInvincible)
        {
            TakeDamage(25); // Missile 충돌
        }
    }

    // 무적 상태 처리 코루틴
    IEnumerator InvincibilityRoutine()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            Debug.Log("Invincible true");
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;

            if (anim != null)
            {
                anim.SetBool(isInvincibleID, false);
            }
            Debug.Log("Invincible false");
        }
    }

    // 패배 애니메이션 처리 루틴
    IEnumerator DefeatedRoutine()
    {
        // 배경 멈춤
        if(WorldScrollers.Length != 0)
        {
            foreach (var scroller in WorldScrollers)
            {
                scroller.scrollSpeed = 0f;
            }
        }

        if (GetComponent<Collider2D>())
        {
            rb2d.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().enabled = false; // 물리 충돌 끄기
        }
        
        Debug.Log("Player Defeated");
        anim.SetBool(isDefeatedID, true);

        yield return new WaitForSeconds(2.5f);
        
        RewardManager.Instance.FinalizeReward(false);
    }

    /// <summary>
    /// 회복 메서드
    /// </summary>
    /// <param name="amount">회복량</param>
    public void Heal(float amount)
    {
        CurrentHP += Mathf.RoundToInt(amount); // 힐
        if (CurrentHP > maxHP) CurrentHP = maxHP; // 최대 체력을 넘지 않도록 함
        playerHPUI.UpdateHP(CurrentHP, maxHP); // UI 업데이트
    }

    /// <summary>
    /// 부활 처리 메서드
    /// </summary>
    /// <param name="hpPrecent">회복량</param>
    public void Resurrect(float hpPrecent)
    {
        CurrentHP = Mathf.RoundToInt(maxHP * hpPrecent); // 회복
        playerHPUI.UpdateHP(CurrentHP, maxHP); // UI 처리
        StartCoroutine(InvincibilityRoutine()); // 부활 후 무적 부여
        // 부활 이펙트 필요
    }
}
