using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

// 로비에서 저장한 데이터를 불러와 UI를 배치하고 입력 감지
public class InGameSquadManager : MonoBehaviour
{
    public SkillSlot SlotQ; // 첫 번째 서포터 UI
    public SkillSlot SlotE; // 두 번째 서포터 UI
    
    private int idQ, idE; // 활성화 스쿼드

    private void Start()
    {
        // 데이터 로드
        idQ = PlayerPrefs.GetInt("Squad_Slot0", -1);
        idE = PlayerPrefs.GetInt("Squad_Slot1", -1);

        // UI 초기화
        //if (idQ != -1) SlotQ.Setup(SupporterDB.Instance.GetSupporter(idQ));
        //if (idE != -1) SlotQ.Setup(SupporterDB.Instance.GetSupporter(idE));
    }


    private void Update()
    {
        // 보스 스테이지 체크 로직 필요
        //if (Keyboard.current.qKey.wasPressedThisFrame && SlotQ.IsReady) StartSkill(idQ, SlotQ); // Q 입력으로 0번 슬롯 서포터 스킬 사용
        //if (Keyboard.current.eKey.wasPressedThisFrame && SlotE.IsReady) StartSkill(idE, SlotE); // E 입력으로 1번 슬롯 서포터 스킬 사용
    }

    /// <summary>
    /// 스킬 시작 메서드
    /// </summary>
    /// <param name="id"></param>
    /// <param name="slot"></param>
    void StartSkill(int id, SkillSlot slot)
    {
        StartCoroutine(SkillSequence(id, slot));
    }

    /// <summary>
    /// 스킬 연출 코루틴
    /// </summary>
    /// <param name="id"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    IEnumerator SkillSequence(int id, SkillSlot slot)
    {
        // 시간 정지 후 스킬 사용 이펙트
        Time.timeScale = 0f;

        // 연출 이펙트 추가 필요

        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        // 스킬 발동
        SkillExecutor.Instance.ExecuteActive(id);

        // 쿨타임
        slot.StartCooldown();
    }

    /// <summary>
    /// 패시브 적용 메서드
    /// </summary>
    /// <param name="id"></param>
    void ApplyPassive(int id)
    {
        //PassiveExecutor.Instance.Apply(id);
    }
}
