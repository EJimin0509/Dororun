using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
    }

    void ApplyPassive(int id)
    {
        //PassiveExecutor.Instance.Apply(id);
    }
}
