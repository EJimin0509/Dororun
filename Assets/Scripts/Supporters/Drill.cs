using UnityEngine;

public class Drill : MonoBehaviour
{
    private float playerBaseDmg;
    private float bonusRatio;

    public void Init(int lv, float bonus)
    {
        int atkLevel = PlayerPrefs.GetInt("Upgrade_ATK_Level", 1);
        playerBaseDmg = 5 + (atkLevel - 1) * 5;
        bonusRatio = bonus;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 적 충돌 시 대미지 (플레이어 현재 공격력 + 보너스)
        if (other.CompareTag("Mob") || other.CompareTag("Boss"))
        {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target != null)
            {
                float finalDmg = playerBaseDmg * (1f + bonusRatio);
                target.TakeDamage(finalDmg);
            }
        }

        // 2. 적 투사체 충돌 시 삭제
        if (other.CompareTag("Bullet"))
        {
            // 플레이어 총알은 무시, 적 총알(EnemyBullet)만 삭제
            if (other.GetComponent<EnemyBullet>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
}