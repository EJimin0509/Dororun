using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f; // 총알 속도
    public float lifeTime = 2f; // 총알 생존 시간
    public float dmg; // 총알 대미지

    void Start()
    {
        Destroy(gameObject, lifeTime); // 일정 시간 후 총알 제거

        // 레벨에 따른 ATK 업데이트
        int hpLevel = PlayerPrefs.GetInt("Upgrade_ATK_Level", 1);
        dmg = (5 + (hpLevel - 1) * 5) * (1f + SkillExecutor.Instance.GetCurrentDmgBonus()); // 4번 서포터 여부
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // 총알 이동
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mob") || other.CompareTag("Boss"))
        {
            BossAI_Whale boss = other.GetComponent<BossAI_Whale>();
            if (boss != null)
            {
                boss.TakeDamage(dmg); // 보스에게 5의 데미지 적용
            }
            MobAI mob = other.GetComponent<MobAI>();
            if (mob != null)
            {
                mob.TakeDamage(dmg);
            }

            // 2번 서포터를 위한 대미지 수치 반환
            if(SkillExecutor.Instance != null)
            {
                SkillExecutor.Instance.ReportDamage(dmg);
            }

            Destroy(gameObject); // 충돌 시 총알 제거
        }     
    }
}
