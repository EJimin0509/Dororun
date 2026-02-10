using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [Header("Currency UI")]
    public TextMeshProUGUI CreditText; // 크레디트 텍스트를 표시
    public TextMeshProUGUI JewelText; // 쥬얼 텍스트를 표시

    private void Start()
    {
        RefreshCurrency();
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
}
