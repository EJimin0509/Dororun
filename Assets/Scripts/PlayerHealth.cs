using UnityEngine;
using UnityEngine.UI;

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

    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerHPUI.UpdateHP(CurrentHP, maxHP);
    }

    // 대미지 처리 메서드
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // 무적 중 실행하지 않음

        CurrentHP -= damage;
        CurrentHP = Mathf.Clamp(CurrentHP, 0, maxHP);

        playerHPUI.UpdateHP(CurrentHP, maxHP); // UI 갱신
        Debug.Log($"dmg: {damage}, HP: {CurrentHP}/{maxHP}");

        if (anim != null) 
        {
            anim.SetTrigger("OnHit");
            anim.SetBool("IsInvincible", true);
        }

        StartCoroutine(InvincibilityRoutine()); // 무적 상태

        if (CurrentHP <= 0) Die(); // 사망
    }

    // 사망 처리
    void Die()
    {
        Debug.Log("Player Dead");
        RewardManager.Instance.FinalizeReward(false);
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
    System.Collections.IEnumerator InvincibilityRoutine()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            Debug.Log("Invincible true");
            yield return new WaitForSeconds(invincibilityTime);
            isInvincible = false;

            isInvincible = false;
            if (anim != null)
            {
                anim.SetBool("IsInvincible", false);
            }
            Debug.Log("Invincible false");
        }
    }
}
