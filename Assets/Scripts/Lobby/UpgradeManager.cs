using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    // HP 세팅
    [Header("HP Settings")]
    public int HpMaxLevel; // HP 최대 레벨
    public int HpBaseCost; // 초기 업그레이드 비용
    public int HpCostIncrease; // 업그레이드 비용 증가량
    public TextMeshProUGUI HpLevelText; // HP 레벨을 보여줄 TMP
    public TextMeshProUGUI HpCostText; // HP 업그레이드 비용을 보여줄 TMP
    public TextMeshProUGUI CurrentHPText; // 현재 HP 수치를 보여줄 TMP
    public Button HpUpgradeButton; // 업그레이드 버튼

    // 공격력 세팅
    [Header("ATK Settings")]
    public int ATKMaxLevel; // ATK 최대 레벨
    public int ATKBaseCost; // 초기 업그레이드 비용
    public int ATKCostIncrease; // 업그레이드 비용 증가량
    public TextMeshProUGUI ATKLevelText; // ATK 레벨을 보여줄 TMP
    public TextMeshProUGUI ATKCostText; // ATK 업그레이드 비용을 보여줄 TMP
    public TextMeshProUGUI CurrentATKText; // 현재 ATK 수치를 보여줄 TMP
    public Button ATKUpgradeButton; // 업그레이드 버튼

    private LobbyManager lobbyManager; // 로비 매니저 인스턴스

    private void Start()
    {
        lobbyManager = FindAnyObjectByType<LobbyManager>(); // LobbyManager가 있는 모든 오브젝트
        UpdateUI();
    }

    /// <summary>
    /// HP 업그레이드 메서드
    /// </summary>
    public void UpgradeHP()
    {
        int currentLv = PlayerPrefs.GetInt("Upgrade_HP_Level", 1); // 현재 저장된 레벨 가져옴
        int cost = GetCost(HpBaseCost, HpCostIncrease, currentLv); // 업그레이드 비용 산출

        if (HandlePurchase(cost) && currentLv < HpMaxLevel) // 업그레이드 시도했을 경우
        {
            PlayerPrefs.SetInt("Upgrade_HP_Level", currentLv + 1); // 레벨 상승
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    /// <summary>
    /// ATK 업그레이드 메서드
    /// </summary>
    public void UpgradeATK()
    {
        int currentLv = PlayerPrefs.GetInt("Upgrade_ATK_Level", 1); // 현재 저장된 레벨
        int cost = GetCost(ATKBaseCost, ATKCostIncrease, currentLv); // 업그레이드 비용 산출

        if (HandlePurchase(cost) && currentLv < ATKMaxLevel) // 업그레이드 시도했을 경우
        {
            PlayerPrefs.SetInt("Upgrade_ATK_Level", currentLv + 1); // 레벨 상승
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    /// <summary>
    /// baseCost와 increase, 현재 레벨을 사용해 업그레이드 비용을 산출하는 메서드
    /// </summary>
    /// <param name="baseCost"></param>
    /// <param name="increase"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private int GetCost(int baseCost, int increase, int level)
    {
        int finalCost = baseCost + (level - 1) * increase; // 업그레이드 비용 산출 공식
        return finalCost;
    }

    /// <summary>
    /// 현재 크레디트를 가져와 구매 가능 여부를 판단하고, 구매시 크레디트를 차감하는 메서드
    /// </summary>
    /// <param name="cost"></param>
    /// <returns></returns>
    private bool HandlePurchase(int cost)
    {
        int totalCredit = PlayerPrefs.GetInt("TotalCredit", 0);
        if  (totalCredit >= cost)
        {
            PlayerPrefs.SetInt("TotalCredit", totalCredit - cost);
            lobbyManager.RefreshCurrency(); // 로비 재화 UI 갱신
            return true;
        }
        Debug.Log("Not Enough Credits!");
        return false;
    }

    public void UpdateUI()
    {
        // HP 업데이트
        int hpLv = PlayerPrefs.GetInt("Upgrade_HP_Level", 1); // 저장된 HP 레벨 가져옴
        int hpCost = GetCost(HpBaseCost, HpCostIncrease, hpLv); // HP 업그레이드 비용 산출
        int currentHP = 100 + (hpLv - 1) * 50; // 현재 HP
        
        HpLevelText.text = $"Lv. {hpLv} / {HpMaxLevel}"; // 현재 레벨 / 최대 레벨 텍스트
        HpCostText.text = hpLv >= HpMaxLevel ? "MAX" : hpCost.ToString("N0"); // 업그레이드 비용 텍스트(레벨 최대일 때 MAX 출력)
        CurrentHPText.text = $"{currentHP}"; // 현재 HP 수치 표시
        
        HpUpgradeButton.interactable = hpLv < HpMaxLevel && PlayerPrefs.GetInt("TotalCredit", 0) >= hpCost; // 버튼 활성화

        // ATK 업데이트
        int atkLv = PlayerPrefs.GetInt("Upgrade_ATK_Level", 1); // 저장된 ATK 레벨 가져옴
        int atkCost = GetCost(ATKBaseCost, ATKCostIncrease, atkLv); // ATK 업그레이드 비용 산출
        int currentATK = 5 + (atkLv - 1) * 5; // 현재 ATK
        
        ATKLevelText.text = $"Lv. {atkLv} / {ATKMaxLevel}"; // 현재 레벨 / 최대 레벨 텍스트
        ATKCostText.text = atkLv >= ATKMaxLevel ? "MAX" : atkCost.ToString("N0"); // 업그레이드 비용 텍스트(레벨 최대일 때 MAX 출력)
        CurrentATKText.text = $"{currentATK}"; // 현재 ATK 수치 표시
        
        ATKUpgradeButton.interactable = atkLv < ATKMaxLevel && PlayerPrefs.GetInt("TotalCredit", 0) >= atkCost; // 버튼 활성화
    }
}
