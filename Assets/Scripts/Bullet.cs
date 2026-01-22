using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f; // 총알 속도
    public float lifeTime = 2f; // 총알 생존 시간

    void Start()
    {
        Destroy(gameObject, lifeTime); // 일정 시간 후 총알 제거
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime); // 총알 이동
    }

    // 충돌 처리
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); // 충돌 시 총알 제거
    }
}
