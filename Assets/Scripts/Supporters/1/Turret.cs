using UnityEngine;

// 1번 서포터 액티브 스킬: 터렛 동작 Class
public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public float fireRate; // 발사 간격

    private float timer; // 발사 간격 제어

    private void Update()
    {
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
        GameObject[] enemise = GameObject.FindGameObjectsWithTag("Enemy"); // 적 오브젝트
        GameObject closest = null; // 가장 가까운 적
        float minDistance = Mathf.Infinity; // 가장 가까운 거리

        foreach (GameObject enemy in enemise)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if(dist < minDistance) // 최소 거리라면
            {
                // 교체
                closest = enemy;
                minDistance = dist;
            }
        }

        // 발사
        if (closest != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector2 dir = (closest.transform.position = transform.position).normalized;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = dir * 10f; // 총알 속도
        }
    }
}
