using UnityEngine;
using UnityEngine.UI;

public class LobbySupporter : MonoBehaviour
{
    // 서포터 및 UI 세팅
    [Header("Supporter Settings")]
    public int SupporterID; // 서포터 ID
    public Sprite LockedSprite; // 획득 전 스프라이트
    public Sprite UnlockedSprite; // 획득 후 스프라이트
    public GameObject InfoPopup; // 서포터 정보 팝업창

    private Image SupporterImage; // 서포터 스프라이트 변경을 위한 이미지 컴포넌트
    private Button SupporterButton; // 서포터를 클릭할 수 있게 하기 위한 버튼 컴포넌트
    private bool isUnlocked = false; // 획득 : 미획득을 판단하기 위한 bool

    private void Start()
    {
        SupporterImage = GetComponent<Image>();
        SupporterButton = GetComponent<Button>();

        CheckUnlock();
    }

    /// <summary>
    /// 서포터 스프라이트를 변경하는 메서드
    /// </summary>
    public void CheckUnlock()
    {
        isUnlocked = PlayerPrefs.GetInt("Supporter_" + SupporterID, 0) == 1; //

        if (isUnlocked) // 획득했을 경우
        {
            SupporterImage.sprite = UnlockedSprite; // 획득 스프라이트로 변경
            SupporterButton.interactable = true; // 선택 가능
        }
        else // 획득하지 못했을 경우
        {
            SupporterImage.sprite = LockedSprite; // 미획득 스프라이트 상태
            SupporterButton.interactable = false; // 선택 불가
        }
    }

    /// <summary>
    /// Button 컴포넌트에 연결 위한 메서드
    /// 서포터 클릭 시 호출
    /// </summary>
    public void OnClickSupporter()
    {
        if (!isUnlocked) return; // 미획득 시 리턴

        InfoPopup.SetActive(true); // 팝업 띄우기
    }
}
