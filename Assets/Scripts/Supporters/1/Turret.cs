using UnityEngine;

// 1번 서포터 액티브 스킬: 터렛 동작 Class
public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public float fireRate; // 발사 간격

    private float timer; // 발사 간격 제어
    private float damage; // 총알 대미지

    /// <summary>
    /// 레벨에 따른 대미지 초기화 메서드
    /// </summary>
    /// <param name="lv">서포터 레벨</param>
    public void Init(int lv)
    {
        damage = 5f + (lv - 1) * 2f;
    }

    private void Update()
    {
        // 발사 간격 조절
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            ShootClosestEnemy();
            timer = 0;
        }
    }

    /// <summary>
    /// 가장 가까운 적을 향해 쏘는 메서드
    /// </summary>
    void ShootClosestEnemy()
    {
        GameObject[] mobs = GameObject.FindGameObjectsWithTag("Mob"); // 적 오브젝트
        GameObject[] boss = GameObject.FindGameObjectsWithTag("Boss"); // 보스 오브젝트

        GameObject closest = null; // 가장 가까운 적
        float minDistance = Mathf.Infinity; // 가장 가까운 거리

        FindClosest(mobs, ref closest, ref minDistance); // 적 탐색
        FindClosest(boss, ref closest, ref minDistance); // 보스 탐색



        // 발사
        if (closest != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector2 dir = (closest.transform.position - transform.position).normalized;

            // Bullet.cs의 대미지 설정
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null) bulletScript.dmg = this.damage;

            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 15f;
        }
    }

    /// <summary>
    /// 가장 가까운 적 찾기
    /// </summary>
    /// <param name="targets">타겟</param>
    /// <param name="closest">가장 가까운 오브젝트</param>
    /// <param name="minDistance">가장 가까운 거리</param>
    void FindClosest(GameObject[] targets, ref GameObject closest, ref float minDistance)
    {
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist < minDistance) // 최소 거리라면
            {
                // 교체
                closest = target;
                minDistance = dist;
            }
        }
    }
}
