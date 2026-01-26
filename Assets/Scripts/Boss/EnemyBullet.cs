using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// BulltType 종류를 나누어 검사 로직 실행
/// </summary>
enum BulletType
{
    None,Bullet,Missile
};

public class EnemyBullet : MonoBehaviour
{
    public float speed = 10f; // 총알 속도
    private Vector2 direction = Vector2.left; // 총알 이동 방향

    [SerializeField] private BulletType type;
    /// <summary>
    /// 폭발 애니메이션 재생 세팅
    /// </summary>
    [Header("Explosion Settings")]
    public bool isExplosive = false; // 폭발 애니메이션 보유하는 프리팹만 적용
    public string explodeTriggerName = "OnExplosion"; // 애니메이션 트리거
    public float explosionDelay = 0.5f; // 폭발 애니메이션 표시 시간

    private Animator anim; // 애니메이션 참조
    private Collider2D col; // 콜라이더
    private bool isExploding = false; //폭발 여부 체크, 정지를 위함

    private void Awake()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }
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
        if(!isExploding) transform.Translate(direction * speed * Time.deltaTime); // 폭발 상태가 아니면 총알 이동
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isExploding) return; // 폭발하면 통과

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by enemy bullet!");

            PlayerHealth player = other.GetComponent<PlayerHealth>(); // HP 연결
            if (player != null)
            {
                player.TakeDamage(10);
            }

            HandleDestruction();    
        }
    }

    // 타입을 검사 && 애니메이션을 적용할 메서드
    private void HandleDestruction()
    {
        switch (type)
        {
            case BulletType.Missile:
                if (anim != null) StartCoroutine(ExplodeAndDestroy()); // 폭발 후 삭제 코루틴 실행
                else Destroy(gameObject); break;
            case BulletType.Bullet:
            case BulletType.None:
            default:
                Destroy(gameObject); break;
        }
    }

    // 폭발 후 삭제 코루틴
    IEnumerator ExplodeAndDestroy()
    {
        isExploding = true;
        if (col != null) col.enabled = false;

        if (anim != null) anim.SetTrigger(explodeTriggerName);

        yield return new WaitForSeconds(explosionDelay);
        Destroy(gameObject);
    }
}
