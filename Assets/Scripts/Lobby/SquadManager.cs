using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SquadManager : MonoBehaviour
{
    // 스테이지에 데려갈 서포터를 골라 스쿼드 만들기
    // 최대 2명
    public List<int> SelectedSquad; // 선택된 ID 리스트
    public Button SaveButton; // 스쿼드 저장 버튼

    private void Start()
    {
        SelectedSquad = new List<int>();
        SaveButton.interactable = false; // 비활성화 상태로 초기화
        LoadSquad();
    }

    /// <summary>
    /// 스쿼드를 만들기 위해 서포터를 토글로 선택하는 메서드 -> 버튼에 할당
    /// 최대 2명
    /// </summary>
    /// <param name="id"></param>
    public void ToggleSelectSupporter(int id)
    {
        if (SelectedSquad.Contains(id)) // 이미 선택된 경우
        {
            SelectedSquad.Remove(id); // 제거
        }
        else
        {
            if (SelectedSquad.Count < 2) // 최대 2개만 선택 가능
            {
                SelectedSquad.Add(id); // Add
            }
            else // 2명을 초과한 경우
            {
                Debug.Log("Maximum Supporters in Squad is 2");
                return;
            }
        }

        SaveButton.interactable = SelectedSquad.Count > 0; // 하나라도 선택되면 활성

        // 추가적인 하이라이트 UI 업데이트 로직 필요
    }

    /// <summary>
    /// 편성할 스쿼드를 PlayerPefs로 저장하는 메서드
    /// </summary>
    public void SaveSquad()
    {
        PlayerPrefs.SetInt("Squad_Slot0", SelectedSquad.Count > 0 ? SelectedSquad[0] : -1); // 하나라도 있으면 Squad_Slot0에 0번 Element 삽입, 없으면 -1
        PlayerPrefs.SetInt("Squad_Slot1", SelectedSquad.Count > 1 ? SelectedSquad[1] : -1); // 하나보다 많이 있으면 Squad_Slot 1번 Element 삽입, 없으면 -1
        PlayerPrefs.Save(); // 저장

        SaveButton.interactable = false; // 저장 후 비활성화
        Debug.Log("Squad Save!");
    }

    /// <summary>
    /// 저장된 스쿼드 불러오기
    /// </summary>
    void LoadSquad()
    {
        int slot0 = PlayerPrefs.GetInt("Squad_Slot0", -1);
        int slot1 = PlayerPrefs.GetInt("Squad_Slot1", -1);

        // 기존에 저장된 스쿼드가 있을 경우 리스트에 포함
        if (slot0 != -1) SelectedSquad.Add(slot0);
        if (slot1 != -1) SelectedSquad.Add(slot1);
    }
}
