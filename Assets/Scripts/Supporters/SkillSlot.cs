using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// 쿨타임 연출
public class SkillSlot : MonoBehaviour
{
    public Image SupporterIcon; // 서포터 아이콘
    public Image CooldownOverlay; // 회색 투명 이미지

    private float cooldownTime;  // 쿨타임
    private float currentCooldown; // 현재 쿨타임

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
