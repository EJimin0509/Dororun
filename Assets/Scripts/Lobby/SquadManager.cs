using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SquadManager : MonoBehaviour
{
    // 스테이지에 데려갈 서포터를 골라 스쿼드 만들기
    // 최대 2명
    public static SquadManager Instance;

    // 서포터 기본 정보 세팅
    [Header("Status")]
    public List<int> SelectedSquad; // 선택된 ID 리스트
    public bool isSquadMode; // 스쿼드 편성 모드인지
    public Sprite SelectedVisual; // 선택 후 비주얼

    // UI 세팅
    [Header("UI Buttons")]
    public Button SquadModeButton; // 스쿼드 편성 버튼
    public Button SaveButton; // 스쿼드 저장 버튼

    private Sprite originSprite; // 선택 전 비주얼

    void Awake() => Instance = this;

    private void Start()
    {
        SelectedSquad = new List<int>();
        SaveButton.gameObject.SetActive(false); // 비활성화 상태로 초기화
        isSquadMode = false;
        LoadSquad();

        originSprite = GetComponent<Sprite>();
    }

    public void ToggleSquadMode()
    {
        isSquadMode = !isSquadMode;

        SaveButton.gameObject.SetActive(isSquadMode);

        Debug.Log(isSquadMode ? "Squad Mode True" : "Squad Mode False");
    }

    /// <summary>
    /// 스쿼드를 만들기 위해 서포터를 토글로 선택하는 메서드 -> 버튼에 할당
    /// 최대 2명
    /// </summary>
    /// <param name = "id" ></ param >
    public bool ToggleSelectSupporter(int id)
    {
        if (PlayerPrefs.GetInt("Supporter_" + id, 0) != 1) return false;

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
            }
        }

        SaveButton.interactable = SelectedSquad.Count > 0; // 하나라도 선택되면 활성

        // 추가적인 하이라이트 UI 업데이트 로직 필요 -> 하이라이트 여부까지 Prefab Save 해야 할 듯

        return true;
    }

    /// <summary>
    /// 편성할 스쿼드를 PlayerPefs로 저장하는 메서드
    /// </summary>
    public void SaveSquad()
    {
        PlayerPrefs.SetInt("Squad_Slot0", SelectedSquad.Count > 0 ? SelectedSquad[0] : -1); // 하나라도 있으면 Squad_Slot0에 0번 Element 삽입, 없으면 -1
        PlayerPrefs.SetInt("Squad_Slot1", SelectedSquad.Count > 1 ? SelectedSquad[1] : -1); // 하나보다 많이 있으면 Squad_Slot 1번 Element 삽입, 없으면 -1
        PlayerPrefs.Save(); // 저장

        isSquadMode = false;
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
