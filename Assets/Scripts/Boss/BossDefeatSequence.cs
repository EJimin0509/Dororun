using UnityEngine;
using System.Collections;

public class BossDefeatSequence : MonoBehaviour
{

    [Header("Settings")]
    public float sinkSpeed = 1.5f; // 가라앉는 속도
    public float sinkDuration = 3f; // 가라앉는 시간
    public float ExplosionTime;

    private Animator anim; // 이팩트 애니메이션
    private bool isDefeated = false; // 보스의 패배 여부

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 패배씬 재생 메서드
    public void StartDefeatSequence()
    {
        if (isDefeated) return; // 패배하지 않았다면 return
        isDefeated = true; // 패배
        StartCoroutine(DefeatRoutione()); // 코루틴 시작
    }

    // 패배씬 코루틴
    IEnumerator DefeatRoutione()
    {
        var player = FindAnyObjectByType<PlayerController>(); // 플레이어 제어권 참조
        if (player != null) player.enabled = false; // 조작 금지

        if (GetComponent<Collider2D>()) GetComponent<Collider2D>().enabled = false; // 충돌 끄기

        anim.Play("ExplosionEffect"); // 폭발 이팩트 애니메이션 재생

        yield return new WaitForSeconds(ExplosionTime); // 폭발 시간

        anim.SetTrigger("ExplosionEnd"); // 연기로 전이

        // 가라앉기
        float timer = 0; // 가라앉는 시간 핸들링 타이머
        while (timer < sinkDuration)
        {
            transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime); // 가라 앉기
            timer += Time.deltaTime; // 타이머 +1
            yield return null;
        }
    }
}
