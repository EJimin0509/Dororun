using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [Header("Currency UI")]
    public TextMeshProUGUI CreditText; // 크레디트 텍스트를 표시
    public TextMeshProUGUI JewelText; // 쥬얼 텍스트를 표시

    [Header("Stage Settings")]
    public StageButton[] StageButtons; // 스테이지 버튼
    public StagePopUp StagePopUp; // 팝업

    private void Start()
    {
        RefreshCurrency();
        UpdateStageUnlockStatus();
    }

    /// <summary>
    /// 재화 UI 업데이트
    /// </summary>
    public void RefreshCurrency()
    {
        int totalCredit = PlayerPrefs.GetInt("TotalCredit", 0); // 크레디트 가져오기, 없으면 0
        int totalJewel = PlayerPrefs.GetInt("TotalJewel", 0); // 쥬얼 가져오기, 없으면 0

        CreditText.text = totalCredit.ToString("N0"); // ,000 포맷팅
        JewelText.text = totalJewel.ToString("N0"); // ,000 포맷팅
    }

    /// <summary>
    /// 스테이지 상태 갱신 메서드
    /// </summary>
    public void UpdateStageUnlockStatus()
    {
        int maxClearedStage = PlayerPrefs.GetInt("MaxClearedStage", 0);

        foreach (StageButton sb in StageButtons)
        {
            // 이전 스테이지를 깼거나 1스테이지라면 해금
            bool isUnlocked = (sb.StageIndex <= maxClearedStage + 1);
            sb.SetStageStatus(isUnlocked);
        }
    }

    /// <summary>
    /// 스테이지 버튼 클릭 시 호출 메서드
    /// </summary>
    /// <param name="index"></param>
    public void RequestStageStart(int index)
    {
        StagePopUp.OpenPopup(index);
    }
}
