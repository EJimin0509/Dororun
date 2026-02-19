using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

// 로비에서 저장한 데이터를 불러와 UI를 배치하고 입력 감지
public class InGameSquadManager : MonoBehaviour
{
    public SkillSlot[] SkillSlots; // UI 슬롯
    
    private int[] activeSquad; // 활성화 스쿼드

    private void Awake()
    {
        activeSquad = new int[2]; // 최대 2명
    }

    private void Start()
    {
        // 데이터 로드
        activeSquad[0] = PlayerPrefs.GetInt("Squad_Slot0", -1);
        activeSquad[1] = PlayerPrefs.GetInt("Squad_Slot1", -1);

        // UI 및 패시브 초기화
        for (int i = 0; i < 2; i++)
        {
            if (activeSquad[i] != -1)
            {
                var data = SupporterDB.Instance.GetSupporter(activeSquad[i]);
                SkillSlots[i].Init(data);
                ApplyPassive(activeSquad[i]);
            }
            else
            {
                SkillSlots[i].gameObject.SetActive(false);
            }
        }
    }


    private void Update()
    {
        //if(!BossStageManager)

        if (Keyboard.current.qKey.wasPressedThisFrame && activeSquad[0] != -1) UseSkill(0); // Q 입력으로 0번 슬롯 서포터 스킬 사용
        if (Keyboard.current.eKey.wasPressedThisFrame && activeSquad[1] != -1) UseSkill(1); // E 입력으로 1번 슬롯 서포터 스킬 사용
    }

    /// <summary>
    /// 스킬 사용 메서드
    /// </summary>
    /// <param name="slotIndex"></param>
    void UseSkill(int slotIndex)
    {
        if (SkillSlots[slotIndex].IsReady()) // 쿨타임이 끝났을 때
        {
            StartCoroutine(SkillSequence(activeSquad[slotIndex], SkillSlots[slotIndex]));
        }
    }

    /// <summary>
    /// 스킬 연출 코루틴
    /// </summary>
    /// <param name="id"></param>
    /// <param name="slot"></param>
    /// <returns></returns>
    IEnumerator SkillSequence(int id, SkillSlot slot)
    {
        Time.timeScale = 0f;

        // 연출 이펙트

        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        SkillExecutor.Instance.ExecuteActive(id);
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
