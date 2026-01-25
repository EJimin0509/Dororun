using UnityEngine;
using UnityEngine.UI;

public class HP_Bar : MonoBehaviour
{
    public Image whiteFill; // 현재 체력(흰색)
    public Image redBuffer; // 대미지 잔상(빨간색)

    public float lerpSpeed = 0.05f; // 빨간 바가 따라오는 속도
    private float targetFillAmount = 1f; // 바 변경 정도

    public void UpdateHP(float currentHP, float maxHP)
    {
        targetFillAmount = currentHP / maxHP; // 목표 비율 계산
        whiteFill.fillAmount = targetFillAmount; // 업데이트 HP
    }

    void Update()
    {
       // redBuffer가 서서히 줄어듦
       if (redBuffer.fillAmount > targetFillAmount)
        {
            redBuffer.fillAmount = Mathf.Lerp(redBuffer.fillAmount, targetFillAmount, lerpSpeed);
        }
    }
}
