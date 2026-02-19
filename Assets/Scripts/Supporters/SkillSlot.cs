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
}
