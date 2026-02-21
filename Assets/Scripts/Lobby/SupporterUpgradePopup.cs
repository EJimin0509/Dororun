using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SupporterUpgradePopup : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI levelText;      // 현재 레벨 표시
    public Button upgradeButton;           // 업그레이드 버튼
    public TextMeshProUGUI costText;       // 소모 쥬얼 표시

    private int currentSupporterID;
    private const int MAX_LEVEL = 5;

    // LobbySupporter에서 팝업을 열 때 호출
    public void SetInfo(int supporterID)
    {
        currentSupporterID = supporterID;
        RefreshUI();
    }

    /// <summary>
    /// 버튼 클릭 -> 업그레이드
    /// </summary>
    public void OnClickUpgrade()
    {
        int currentLevel = PlayerPrefs.GetInt($"Upgrade_Supporter_{currentSupporterID}_Level", 1);
        int totalJewel = PlayerPrefs.GetInt("TotalJewel", 0);

        if (currentLevel < MAX_LEVEL && totalJewel >= 1)
        {
            // 재화 차감 및 레벨업
            PlayerPrefs.SetInt("TotalJewel", totalJewel - 1);
            PlayerPrefs.SetInt($"Upgrade_Supporter_{currentSupporterID}_Level", currentLevel + 1);
            PlayerPrefs.Save();

            // 로비의 재화 텍스트 즉시 갱신
            Object.FindAnyObjectByType<LobbyManager>().RefreshCurrency();

            RefreshUI();
            Debug.Log($"ID {currentSupporterID} Upgrade!");
        }
    }

    /// <summary>
    /// UI 초기화
    /// </summary>
    private void RefreshUI()
    {
        int level = PlayerPrefs.GetInt($"Upgrade_Supporter_{currentSupporterID}_Level", 1);
        levelText.text = level >= MAX_LEVEL ? "Lv. MAX" : $"Lv. {level}";

        // 버튼 활성화 조건: 만렙이 아니고 쥬얼이 1개 이상일 때
        upgradeButton.interactable = (level < MAX_LEVEL && PlayerPrefs.GetInt("TotalJewel", 0) >= 1);

        if (costText != null) costText.text = "1";
    }
}