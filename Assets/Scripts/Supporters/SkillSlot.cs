using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 쿨타임 연출
public class SkillSlot : MonoBehaviour
{
    public Image SupporterIcon; // 서포터 아이콘
    public Image CooldownOverlay; // 회색 투명 이미지

    public CanvasGroup canvasGroup; // 투명도 조절용

    private float cooldownTime;  // 쿨타임
    private float currentCooldown; // 현재 쿨타임

    public void SetSlotActive(bool isActive)
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

        // 비활성 시 투명도 0.3, 활성 시 1.0
        canvasGroup.alpha = isActive ? 1f : 0.3f;

        if (isActive)
        {
            // 활성화될 때 살짝 커졌다 작아지는 연출
            transform.localScale = Vector3.one * 1.2f;
            StartCoroutine(ScaleBack());
        }
    }

    IEnumerator ScaleBack()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 5f;
            transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, t);
            yield return null;
        }
    }

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    /// <param name="data"></param>
    public void Init(SupporterData data)
    {
        SupporterIcon.sprite = data.Icon;
        cooldownTime = data.Cooldown;
        CooldownOverlay.fillAmount = 0;
    }

    /// <summary>
    /// 쿨타임 시작 메서드
    /// </summary>
    public void StartCooldown()
    {
        currentCooldown = cooldownTime;
        StartCoroutine(CooldownRoutine()); // 쿨타임 시작
    }

    /// <summary>
    /// 쿨타임 종료 여부 판단 메서드
    /// </summary>
    /// <returns></returns>
    public bool IsReady() => currentCooldown <= 0;

    /// <summary>
    /// 쿨타임 루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator CooldownRoutine()
    {
        while (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            CooldownOverlay.fillAmount = currentCooldown / cooldownTime; // 쿨타임 오버레이 감소
            yield return null;
        }
        CooldownOverlay.fillAmount = 0; // 쿨타임 오버레이 완전히 없앰
    }
}
