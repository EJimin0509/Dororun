using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LobbySupporter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // 서포터 및 UI 세팅
    [Header("Supporter Settings")]
    public int SupporterID; // 서포터 ID
    public Sprite LockedSprite; // 획득 전 스프라이트
    public Sprite UnlockedSprite; // 획득 후 스프라이트
    public Sprite WhiteHighlight; // 마우스 오버용
    public Sprite YellowHighlight; // 선택/저장용
    public GameObject InfoPopup; // 서포터 정보 팝업창

    private Image supporterImage; // 서포터 스프라이트 변경을 위한 이미지 컴포넌트
    private Button supporterButton; // 서포터를 클릭할 수 있게 하기 위한 버튼 컴포넌트
    private bool isUnlocked; // 획득 : 미획득을 판단하기 위한 bool

    private void Awake()
    {
        supporterImage = GetComponent<Image>();
        supporterButton = GetComponent<Button>();
    }

    private void Start()
    {
        isUnlocked = false;

        CheckUnlock();
        UpdateHighlight();
    }

    /// <summary>
    /// 서포터 스프라이트를 변경하는 메서드
    /// </summary>
    public void CheckUnlock()
    {
        isUnlocked = PlayerPrefs.GetInt("Supporter_" + SupporterID, 0) == 1;

        if (isUnlocked) // 획득했을 경우
        {
            supporterImage.sprite = UnlockedSprite; // 획득 스프라이트로 변경
            supporterButton.interactable = true; // 선택 가능
        }
        else // 획득하지 못했을 경우
        {
            supporterImage.sprite = LockedSprite; // 미획득 스프라이트 상태
            supporterButton.interactable = false; // 선택 불가
        }
    }

    /// <summary>
    /// Button 컴포넌트에 연결 위한 메서드
    /// 서포터 클릭 시 호출
    /// 스쿼드 편성 모드에 따라 스쿼드 편성 or 팝업 띄우기
    /// </summary>
    public void OnClickSupporter()
    {
        if (!isUnlocked) return; // 미획득 시 리턴

        if (SquadManager.Instance.isSquadMode)
        {
            if (SquadManager.Instance.ToggleSelectSupporter(SupporterID))
            {
                UpdateHighlight();
            }
        }
        else
        {
            OpenInfoPopup();
        }
    }

    /// <summary>
    /// 마우스를 올렸을 때
    /// </summary>
    /// <param name="enventData"></param>
    public void OnPointerEnter(PointerEventData enventData)
    {
        if (!isUnlocked || SquadManager.Instance.SelectedSquad.Contains(SupporterID)) return; // 미획득 했거나, 스쿼드에 있으면 리턴

        supporterImage.sprite = WhiteHighlight; // 하이라이트
    }

    /// <summary>
    /// 마우스가 나갔을 때
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isUnlocked) return; // 미획득 시 리턴
        UpdateHighlight();
    }

    /// <summary>
    /// 하이라이트를 띄울 메서드
    /// </summary>
    public void UpdateHighlight()
    {
        if (!isUnlocked) return;

        if (SquadManager.Instance.SelectedSquad.Contains(SupporterID)) // 스쿼드에 포함 된 경우
        {
            supporterImage.sprite = YellowHighlight;
        }
        else
        {
            supporterImage.sprite = UnlockedSprite;
        }
    }

    /// <summary>
    /// 팝업을 띄우는 메서드
    /// </summary>
    void OpenInfoPopup()
    {
        InfoPopup.SetActive(true); // 팝업 띄우기

        // 팝업에 스크립트가 있다면 ID 전달
        var upgradePopup = InfoPopup.GetComponent<SupporterUpgradePopup>();
        if (upgradePopup != null)
        {
            upgradePopup.SetInfo(SupporterID);
        }
    }
}
