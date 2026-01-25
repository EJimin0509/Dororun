using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f; // 총알 속도
    private Vector2 direction = Vector2.left; // 총알 이동 방향

    void Start()
    {
        Destroy(gameObject, 4f); // 4초 후 총알 제거
    }

    // 조준 호출
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); // 총알 이동
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy bullet!");
            Destroy(gameObject); // 충돌 시 총알 제거

            PlayerHealth player = other.GetComponent<PlayerHealth>(); // HP 연결
            if (player != null)
            {
                player.TakeDamage(10);
            }
        }
    }
}
